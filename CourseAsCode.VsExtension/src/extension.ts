import * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import type * as yamlTypes from 'js-yaml';
import { initCourseCommand } from './commands/initCourse';
import { updateSectionsCommand } from './commands/updateSections';
import { createActivityCommand } from './commands/createActivity';
import { createSectionCommand } from './commands/createSection';
import { createQuestionCommand } from './commands/createQuestion';
import { generateCourseFileCommand } from './commands/generateCourseFile';
import { CourseExplorerProvider, SectionItem, ActivityItem } from './providers/courseExplorerProvider';
import { registerSectionWatcher } from './utils/sectionWatcher';
import { validateDocument } from './utils/cascadeValidator';
import { RefCompletionProvider } from './providers/refCompletionProvider';
import { getWorkspaceRoot } from './utils/workspace';
import { sanitizeName } from './templates';

// ─── StatusBar ────────────────────────────────────────────────────────────────

function getCourseName(root: string): string {
  const yaml = require('js-yaml') as typeof yamlTypes;
  const configPath = path.join(root, 'config.yaml');
  try {
    if (fs.existsSync(configPath)) {
      const parsed = yaml.load(fs.readFileSync(configPath, 'utf8')) as { CourseName?: string } | null;
      if (parsed?.CourseName) return parsed.CourseName;
    }
  } catch { /* ignore */ }
  return 'CourseAsCode';
}

function getCourseStats(root: string): { sections: number; activities: number } {
  const yaml = require('js-yaml') as typeof yamlTypes;
  try {
    const sectionsRaw = yaml.load(fs.readFileSync(path.join(root, 'sections.yaml'), 'utf8')) as {
      Sections?: Array<{ SectionPath: string }>;
    } | null;
    const entries = sectionsRaw?.Sections ?? [];
    let activityCount = 0;
    for (const entry of entries) {
      try {
        const sectionYaml = yaml.load(
          fs.readFileSync(path.join(root, entry.SectionPath, 'section.yaml'), 'utf8')
        ) as { Activities?: unknown[] } | null;
        activityCount += sectionYaml?.Activities?.length ?? 0;
      } catch { /* ignore missing section */ }
    }
    return { sections: entries.length, activities: activityCount };
  } catch {
    return { sections: 0, activities: 0 };
  }
}

function createStatusBarItem(context: vscode.ExtensionContext): void {
  const item = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 100);
  item.command = 'courseascodetools.showCourseOverview';
  item.tooltip = 'Click to open Course Overview';
  context.subscriptions.push(item);

  function update() {
    const root = getWorkspaceRoot();
    if (root && fs.existsSync(path.join(root, 'config.yaml'))) {
      const { sections, activities } = getCourseStats(root);
      item.text = `$(book) ${getCourseName(root)}  $(layers) ${sections}  $(file) ${activities}`;
      item.tooltip = `${getCourseName(root)} · ${sections} section${sections === 1 ? '' : 's'} · ${activities} activit${activities === 1 ? 'y' : 'ies'} — Click to open Course Overview`;
      item.show();
    } else {
      item.hide();
    }
  }

  update();
  context.subscriptions.push(
    vscode.workspace.onDidChangeWorkspaceFolders(update),
    vscode.workspace.onDidSaveTextDocument(doc => {
      if (doc.fileName.endsWith('.yaml')) update();
    })
  );
}

// ─── Rename Section ───────────────────────────────────────────────────────────

async function renameSectionCommand(item?: SectionItem): Promise<void> {
  const yaml = require('js-yaml') as typeof yamlTypes;

  const root = getWorkspaceRoot();
  if (!root) {
    vscode.window.showErrorMessage('No workspace folder open.');
    return;
  }

  // If called from command palette (no item), let user pick a section
  let sectionDir: string;
  if (item) {
    sectionDir = item.sectionDir;
  } else {
    const sectionsDir = path.join(root, 'Sections');
    if (!fs.existsSync(sectionsDir)) {
      vscode.window.showErrorMessage('No Sections directory found.');
      return;
    }
    const sections = fs.readdirSync(sectionsDir)
      .filter(n => fs.statSync(path.join(sectionsDir, n)).isDirectory());
    const picked = await vscode.window.showQuickPick(sections, { placeHolder: 'Select section to rename' });
    if (!picked) return;
    sectionDir = path.join(sectionsDir, picked);
  }

  const oldFolderName = path.basename(sectionDir);
  const newRawName = await vscode.window.showInputBox({
    prompt: 'Enter new section name',
    value: oldFolderName,
  });
  if (!newRawName) return;

  const newFolderName = sanitizeName(newRawName.trim());
  if (!newFolderName) {
    vscode.window.showErrorMessage('Invalid section name after sanitization.');
    return;
  }
  if (newFolderName === oldFolderName) {
    vscode.window.showInformationMessage('Name unchanged.');
    return;
  }

  const newSectionDir = path.join(path.dirname(sectionDir), newFolderName);
  if (fs.existsSync(newSectionDir)) {
    vscode.window.showErrorMessage(`A section named "${newFolderName}" already exists.`);
    return;
  }

  try {
    // 1. Rename folder on disk
    fs.renameSync(sectionDir, newSectionDir);

    // 2. Update SectionName in section.yaml
    const sectionYamlPath = path.join(newSectionDir, 'section.yaml');
    if (fs.existsSync(sectionYamlPath)) {
      const parsed = yaml.load(fs.readFileSync(sectionYamlPath, 'utf8')) as Record<string, unknown> | null ?? {};
      parsed['SectionName'] = newRawName.trim();
      fs.writeFileSync(sectionYamlPath, yaml.dump(parsed), 'utf8');
    }

    // 3. Update SectionPath in sections.yaml
    const sectionsFile = path.join(root, 'sections.yaml');
    if (fs.existsSync(sectionsFile)) {
      const sectionsData = yaml.load(fs.readFileSync(sectionsFile, 'utf8')) as {
        Sections?: Array<{ SectionPath: string }>;
      } | null ?? {};
      const oldRelPath = `Sections/${oldFolderName}`;
      const newRelPath = `Sections/${newFolderName}`;
      if (sectionsData.Sections) {
        sectionsData.Sections = sectionsData.Sections.map(entry =>
          entry.SectionPath === oldRelPath ? { ...entry, SectionPath: newRelPath } : entry
        );
        fs.writeFileSync(sectionsFile, yaml.dump(sectionsData), 'utf8');
      }
    }

    vscode.window.showInformationMessage(`Section renamed to "${newRawName.trim()}".`);
  } catch (err) {
    vscode.window.showErrorMessage(
      `Failed to rename section: ${err instanceof Error ? err.message : String(err)}`
    );
  }
}

// ─── Duplicate Activity ───────────────────────────────────────────────────────

async function duplicateActivityCommand(item?: ActivityItem): Promise<void> {
  const yaml = require('js-yaml') as typeof yamlTypes;

  const root = getWorkspaceRoot();
  if (!root) {
    vscode.window.showErrorMessage('No workspace folder open.');
    return;
  }

  let activityPath: string;
  if (item?.resourceUri) {
    activityPath = item.resourceUri.fsPath;
  } else if (item && 'command' in item && Array.isArray((item as any).command?.arguments)) {
    activityPath = (item as any).command.arguments[0]?.fsPath ?? '';
  } else {
    vscode.window.showErrorMessage('Please right-click an activity in the Course Explorer to duplicate it.');
    return;
  }

  if (!activityPath || !fs.existsSync(activityPath)) {
    vscode.window.showErrorMessage('Activity file not found.');
    return;
  }

  const originalName = path.basename(activityPath, '.yaml');
  const newRawName = await vscode.window.showInputBox({
    prompt: 'Enter name for the duplicated activity (without extension)',
    value: `${originalName}-copy`,
  });
  if (!newRawName) return;

  const newFileName = `${sanitizeName(newRawName.trim())}.yaml`;
  const activitiesDir = path.dirname(activityPath);
  const newFilePath = path.join(activitiesDir, newFileName);

  if (fs.existsSync(newFilePath)) {
    vscode.window.showErrorMessage(`Activity "${newFileName}" already exists.`);
    return;
  }

  try {
    // 1. Copy the file
    fs.copyFileSync(activityPath, newFilePath);

    // 2. Register in section.yaml
    const sectionDir = path.dirname(activitiesDir);
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');
    if (fs.existsSync(sectionYamlPath)) {
      const parsed = yaml.load(fs.readFileSync(sectionYamlPath, 'utf8')) as {
        SectionName?: string;
        Activities?: Array<{ ActivityFilePath: string }>;
      } | null ?? {};
      if (!parsed.Activities) parsed.Activities = [];
      parsed.Activities.push({ ActivityFilePath: `Activities/${newFileName}` });
      fs.writeFileSync(sectionYamlPath, yaml.dump(parsed), 'utf8');
    }

    // 3. Open the new file
    const doc = await vscode.workspace.openTextDocument(newFilePath);
    await vscode.window.showTextDocument(doc);
    vscode.window.showInformationMessage(`Duplicated as "${newFileName}".`);
  } catch (err) {
    vscode.window.showErrorMessage(
      `Failed to duplicate activity: ${err instanceof Error ? err.message : String(err)}`
    );
  }
}

// ─── Course Overview Webview ──────────────────────────────────────────────────

const ACTIVITY_TYPE_META: Record<string, { label: string; color: string }> = {
  'Quiz_':       { label: 'Quiz',       color: '#7C6AF7' },
  'QUIZ_':       { label: 'Quiz',       color: '#7C6AF7' },
  'Forum_':      { label: 'Forum',      color: '#2E9E6B' },
  'Assignment_': { label: 'Assignment', color: '#D4860A' },
  'ASN_':        { label: 'Assignment', color: '#D4860A' },
  'URL_':        { label: 'URL',        color: '#1A8CD8' },
  'FILE_':       { label: 'File',       color: '#6B7280' },
  'TMA_':        { label: 'Label',      color: '#9D5BB5' },
  'FLD_':        { label: 'Folder',     color: '#C97B30' },
  'WKI_':        { label: 'Wiki',       color: '#3B82F6' },
  'Page_':       { label: 'Page',       color: '#0891B2' },
  'PGE_':        { label: 'Page',       color: '#0891B2' },
  'CHO_':        { label: 'Choice',     color: '#E0658A' },
  'FBK_':        { label: 'Feedback',   color: '#16A394' },
};

function getActivityMeta(fileName: string): { label: string; color: string } {
  for (const [prefix, meta] of Object.entries(ACTIVITY_TYPE_META)) {
    if (fileName.startsWith(prefix)) return meta;
  }
  return { label: 'Activity', color: '#6B7280' };
}

function getCourseOverviewHtml(root: string): string {
  const yaml = require('js-yaml') as typeof yamlTypes;

  // Read course config
  let courseName = 'Course Overview';
  let shortName = '';
  let summary = '';
  try {
    const cfg = yaml.load(fs.readFileSync(path.join(root, 'config.yaml'), 'utf8')) as {
      CourseName?: string; ShortName?: string; Summary?: string;
    } | null;
    if (cfg?.CourseName) courseName = cfg.CourseName;
    if (cfg?.ShortName) shortName = cfg.ShortName;
    if (cfg?.Summary) summary = cfg.Summary;
  } catch { /* ignore */ }

  // Read sections
  let sectionEntries: Array<{ SectionPath: string }> = [];
  try {
    const sectionsRaw = yaml.load(fs.readFileSync(path.join(root, 'sections.yaml'), 'utf8')) as {
      Sections?: Array<{ SectionPath: string }>;
    } | null;
    sectionEntries = sectionsRaw?.Sections ?? [];
  } catch { /* ignore */ }

  let totalActivities = 0;

  // Build section blocks
  let sectionsHtml = '';
  sectionEntries.forEach((entry, idx) => {
    const sectionPath = entry?.SectionPath;
    if (typeof sectionPath !== 'string') return;

    const absDir = path.join(root, sectionPath);
    let sectionName = path.basename(sectionPath);
    let activities: Array<{ ActivityFilePath: string }> = [];
    try {
      const sec = yaml.load(fs.readFileSync(path.join(absDir, 'section.yaml'), 'utf8')) as {
        SectionName?: string; Activities?: Array<{ ActivityFilePath: string }>;
      } | null;
      if (sec?.SectionName) sectionName = sec.SectionName;
      activities = sec?.Activities ?? [];
    } catch { /* ignore */ }

    totalActivities += activities.length;

    const activitiesHtml = activities.map(a => {
      const relPath = a?.ActivityFilePath;
      if (!relPath) return '';
      const absPath = path.join(absDir, relPath);
      const fileName = path.basename(relPath, '.yaml');
      const exists = fs.existsSync(absPath);
      const meta = getActivityMeta(path.basename(relPath));
      // Strip prefix from display name
      const displayName = fileName.replace(/^(Quiz_|QUIZ_|Forum_|Assignment_|ASN_|URL_|FILE_|TMA_|FLD_|WKI_|Page_|PGE_|CHO_|FBK_)/, '');
      return `<li class="activity${exists ? '' : ' missing'}" data-path="${absPath}">
        <span class="type-badge" style="background:${meta.color}22;color:${meta.color};border:1px solid ${meta.color}44">${meta.label}</span>
        <span class="activity-name">${displayName}</span>
        ${exists ? '' : '<span class="missing-badge">⚠ missing</span>'}
      </li>`;
    }).join('');

    sectionsHtml += `
      <div class="section">
        <div class="section-header" data-path="${path.join(absDir, 'section.yaml')}">
          <span class="section-num">${idx + 1}</span>
          <span class="section-name">${sectionName}</span>
          <span class="activity-count">${activities.length} activit${activities.length === 1 ? 'y' : 'ies'}</span>
        </div>
        <ul class="activities">${activitiesHtml || '<li class="empty">No activities yet — right-click in the Course Explorer to add one.</li>'}</ul>
      </div>`;
  });

  const statsHtml = `
    <div class="stats">
      <div class="stat"><span class="stat-num">${sectionEntries.length}</span><span class="stat-label">Sections</span></div>
      <div class="stat"><span class="stat-num">${totalActivities}</span><span class="stat-label">Activities</span></div>
    </div>`;

  return `<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>Course Overview</title>
<style>
  *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
  body { font-family: var(--vscode-font-family); color: var(--vscode-foreground); background: var(--vscode-editor-background); padding: 28px 32px; }

  .course-header { margin-bottom: 24px; padding-bottom: 20px; border-bottom: 1px solid var(--vscode-panel-border); }
  .course-title { display: flex; align-items: center; gap: 12px; margin-bottom: 6px; }
  h1 { font-size: 1.5em; font-weight: 600; }
  .short-name { font-size: 0.8em; padding: 2px 8px; border-radius: 99px; background: var(--vscode-badge-background); color: var(--vscode-badge-foreground); font-weight: 500; }
  .summary { font-size: 0.88em; color: var(--vscode-descriptionForeground); margin-bottom: 16px; }
  .hint { font-size: 0.8em; color: var(--vscode-descriptionForeground); margin-top: 6px; }

  .stats { display: flex; gap: 16px; margin-top: 4px; }
  .stat { display: flex; flex-direction: column; align-items: center; background: var(--vscode-sideBar-background); border: 1px solid var(--vscode-panel-border); border-radius: 8px; padding: 10px 20px; min-width: 80px; }
  .stat-num { font-size: 1.6em; font-weight: 700; line-height: 1; }
  .stat-label { font-size: 0.75em; color: var(--vscode-descriptionForeground); margin-top: 2px; }

  .sections { display: flex; flex-direction: column; gap: 10px; }
  .section { border: 1px solid var(--vscode-panel-border); border-radius: 8px; overflow: hidden; transition: box-shadow 0.15s; }
  .section:hover { box-shadow: 0 2px 8px rgba(0,0,0,0.15); }
  .section-header { display: flex; align-items: center; gap: 10px; padding: 11px 16px; background: var(--vscode-sideBar-background); cursor: pointer; user-select: none; }
  .section-header:hover { background: var(--vscode-list-hoverBackground); }
  .section-num { width: 22px; height: 22px; border-radius: 50%; background: var(--vscode-badge-background); color: var(--vscode-badge-foreground); font-size: 0.75em; font-weight: 600; display: flex; align-items: center; justify-content: center; flex-shrink: 0; }
  .section-name { font-weight: 500; flex: 1; font-size: 0.95em; }
  .activity-count { font-size: 0.78em; color: var(--vscode-descriptionForeground); }

  .activities { list-style: none; }
  .activity { display: flex; align-items: center; gap: 10px; padding: 8px 16px 8px 24px; cursor: pointer; border-top: 1px solid var(--vscode-panel-border); transition: background 0.1s; }
  .activity:hover:not(.missing) { background: var(--vscode-list-hoverBackground); }
  .activity.missing { opacity: 0.55; cursor: default; }
  .type-badge { font-size: 0.72em; padding: 2px 7px; border-radius: 99px; font-weight: 600; white-space: nowrap; flex-shrink: 0; }
  .activity-name { font-size: 0.88em; flex: 1; }
  .missing-badge { font-size: 0.75em; color: var(--vscode-inputValidation-warningForeground); }
  .empty { padding: 10px 16px 10px 24px; color: var(--vscode-descriptionForeground); font-size: 0.82em; border-top: 1px solid var(--vscode-panel-border); font-style: italic; }
</style>
</head>
<body>
<div class="course-header">
  <div class="course-title">
    <h1>📚 ${courseName}</h1>
    ${shortName ? `<span class="short-name">${shortName}</span>` : ''}
  </div>
  ${summary ? `<p class="summary">${summary}</p>` : ''}
  ${statsHtml}
  <p class="hint">Click any section or activity to open it in the editor.</p>
</div>
<div class="sections">
${sectionsHtml || '<p style="color:var(--vscode-descriptionForeground)">No sections found. Run <strong>Init Course</strong> first.</p>'}
</div>
<script>
  const vscode = acquireVsCodeApi();
  document.querySelectorAll('.section-header, .activity:not(.missing)').forEach(el => {
    el.addEventListener('click', () => {
      const p = el.getAttribute('data-path');
      if (p) vscode.postMessage({ command: 'open', path: p });
    });
  });
</script>
</body>
</html>`;
}

async function showCourseOverviewCommand(): Promise<void> {
  const root = getWorkspaceRoot();
  if (!root) {
    vscode.window.showErrorMessage('No workspace folder open.');
    return;
  }
  if (!fs.existsSync(path.join(root, 'config.yaml'))) {
    vscode.window.showErrorMessage('No course found. Run Init Course first.');
    return;
  }

  const panel = vscode.window.createWebviewPanel(
    'courseascodetools.overview',
    'Course Overview',
    vscode.ViewColumn.One,
    { enableScripts: true }
  );

  panel.webview.html = getCourseOverviewHtml(root);

  // Handle open-file messages from the webview
  panel.webview.onDidReceiveMessage(msg => {
    if (msg.command === 'open' && typeof msg.path === 'string') {
      vscode.workspace.openTextDocument(msg.path).then(doc => vscode.window.showTextDocument(doc));
    }
  });

  // Refresh when a YAML file is saved
  const sub = vscode.workspace.onDidSaveTextDocument(doc => {
    if (doc.fileName.endsWith('.yaml')) {
      panel.webview.html = getCourseOverviewHtml(root);
    }
  });
  panel.onDidDispose(() => sub.dispose());
}

// ─── activate ────────────────────────────────────────────────────────────────

export function activate(context: vscode.ExtensionContext): void {
  // Warn if executable path is not configured
  const config = vscode.workspace.getConfiguration('courseascodetools');
  if (!config.get<string>('executablePath')) {
    vscode.window.showWarningMessage(
      'CourseAsCode: executable path is not set. Please configure "courseascodetools.executablePath" in Settings.',
      'Open Settings'
    ).then(choice => {
      if (choice === 'Open Settings') {
        vscode.commands.executeCommand('workbench.action.openSettings', 'courseascodetools.executablePath');
      }
    });
  }

  const provider = new CourseExplorerProvider();
  vscode.window.registerTreeDataProvider('courseascodetools.explorer', provider);

  registerSectionWatcher(context, () => provider.refresh());

  // StatusBar
  createStatusBarItem(context);

  context.subscriptions.push(
    vscode.languages.registerCompletionItemProvider(
      { language: 'yaml' },
      new RefCompletionProvider(),
      ':'
    )
  );

  const diagnostics = vscode.languages.createDiagnosticCollection('courseascodetools');
  context.subscriptions.push(diagnostics);

  context.subscriptions.push(
    vscode.workspace.onDidOpenTextDocument(doc => validateDocument(doc, diagnostics)),
    vscode.workspace.onDidChangeTextDocument(e => validateDocument(e.document, diagnostics)),
    vscode.workspace.onDidCloseTextDocument(doc => diagnostics.delete(doc.uri))
  );

  vscode.workspace.textDocuments.forEach(doc => validateDocument(doc, diagnostics));

  context.subscriptions.push(
    vscode.commands.registerCommand(
      'courseascodetools.initCourse',
      (folderUri?: vscode.Uri) => initCourseCommand(folderUri)
    ),
    vscode.commands.registerCommand(
      'courseascodetools.updateSections',
      () => updateSectionsCommand()
    ),
    vscode.commands.registerCommand(
      'courseascodetools.createActivity',
      () => createActivityCommand()
    ),
    vscode.commands.registerCommand(
      'courseascodetools.createSection',
      () => createSectionCommand()
    ),
    vscode.commands.registerCommand(
      'courseascodetools.createQuestion',
      () => createQuestionCommand()
    ),
    vscode.commands.registerCommand(
      'courseascodetools.refreshExplorer',
      () => provider.refresh()
    ),
    // New commands
    vscode.commands.registerCommand(
      'courseascodetools.renameSection',
      (item?: SectionItem) => renameSectionCommand(item)
    ),
    vscode.commands.registerCommand(
      'courseascodetools.duplicateActivity',
      (item?: ActivityItem) => duplicateActivityCommand(item)
    ),
    vscode.commands.registerCommand(
      'courseascodetools.showCourseOverview',
      () => showCourseOverviewCommand()
    ),
    vscode.commands.registerCommand(
      'courseascodetools.generateCourseFile',
      () => generateCourseFileCommand(context.extensionPath)
    ),
    vscode.commands.registerCommand(
      'courseascodetools.revealRelease',
      (item: { resourceUri?: vscode.Uri }) => {
        if (item?.resourceUri) {
          return vscode.commands.executeCommand('revealFileInOS', item.resourceUri);
        }
      }
    ),
  );
}

export function deactivate(): void {}