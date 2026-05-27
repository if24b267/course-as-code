import type * as vscode from 'vscode';
import type * as yamlTypes from 'js-yaml';
import * as fs from 'fs';
import * as path from 'path';
import { ActivityType, buildFileName, sanitizeName, getTemplate } from '../templates';
import { getWorkspaceRoot } from '../utils/workspace';

const ACTIVITY_TYPES: ActivityType[] = ['Forum', 'Page', 'Quiz', 'Assignment', 'Url', 'File', 'Label', 'Folder', 'Wiki'];

interface SectionYaml {
  SectionName?: string;
  Activities?: Array<{ ActivityFilePath: string }>;
}

export function listSections(root: string): string[] {
  const sectionsDir = path.join(root, 'Sections');
  if (!fs.existsSync(sectionsDir)) {
    return [];
  }
  return fs.readdirSync(sectionsDir)
    .filter(name => fs.statSync(path.join(sectionsDir, name)).isDirectory())
    .map(name => `Sections/${name}`);
}

/**
 * Builds the ActivityFilePath entry for section.yaml.
 * Takes the raw (unsanitized) activity name — sanitization is handled by buildFileName internally.
 */
export function buildActivityEntry(type: ActivityType, rawName: string): string {
  return `Activities/${buildFileName(type, rawName)}`;
}

export function appendActivityToSection(sectionDir: string, activityFilePath: string): void {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;

  const sectionYamlPath = path.join(sectionDir, 'section.yaml');
  const raw = fs.readFileSync(sectionYamlPath, 'utf8');

  // Throws if YAML is unparseable — caller must catch and show error.
  const parsed = yaml.load(raw) as SectionYaml;
  if (!parsed.Activities) {
    parsed.Activities = [];
  }
  parsed.Activities.push({ ActivityFilePath: activityFilePath });

  // NOTE: yaml.dump() re-serializes the document, which strips any inline comments
  // the user may have added to section.yaml. This is a known limitation of this pass —
  // comment-preserving round-trip YAML is out of scope.
  fs.writeFileSync(sectionYamlPath, yaml.dump(parsed), 'utf8');
}

export async function createActivityCommand(): Promise<void> {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  const root = getWorkspaceRoot();
  if (!root) {
    vs.window.showErrorMessage('No workspace folder open.');
    return;
  }

  // Step 1: Section picker
  const sections = listSections(root);
  if (sections.length === 0) {
    vs.window.showErrorMessage('No sections found. Run Init Course first.');
    return;
  }

  const selectedSection = await vs.window.showQuickPick(sections, {
    placeHolder: 'Select a section',
  });
  if (!selectedSection) {
    return;
  }

  // Step 2: Activity type picker
  const selectedType = await vs.window.showQuickPick(ACTIVITY_TYPES as string[], {
    placeHolder: 'Select activity type',
  }) as ActivityType | undefined;
  if (!selectedType) {
    return;
  }

  // Step 3: Name input + validation
  const rawName = await vs.window.showInputBox({
    prompt: `Enter a name for the ${selectedType} activity`,
    placeHolder: 'e.g. general-discussion',
  });
  if (rawName === undefined) {
    return;
  }

  const sanitized = sanitizeName(rawName.trim());
  if (!sanitized) {
    vs.window.showErrorMessage(
      'Activity name is empty after sanitization. Use letters, digits, hyphens, or underscores.'
    );
    return;
  }

  // Step 4: Generate files
  const sectionDir = path.join(root, selectedSection);
  const activitiesDir = path.join(sectionDir, 'Activities');
  if (!fs.existsSync(activitiesDir)) {
    fs.mkdirSync(activitiesDir, { recursive: true });
  }

  const fileName = buildFileName(selectedType, sanitized);
  const activityFilePath = path.join(activitiesDir, fileName);

  if (fs.existsSync(activityFilePath)) {
    vs.window.showErrorMessage(`Activity file already exists: ${fileName}`);
    return;
  }

  fs.writeFileSync(activityFilePath, getTemplate(selectedType), 'utf8');

  try {
    appendActivityToSection(sectionDir, `Activities/${fileName}`);
  } catch {
    vs.window.showErrorMessage(
      `section.yaml in ${selectedSection} is not valid YAML. Activity file was created but not registered.`
    );
    return;
  }

  const doc = await vs.workspace.openTextDocument(activityFilePath);
  await vs.window.showTextDocument(doc);
  vs.window.showInformationMessage(`Created ${fileName} in ${selectedSection}`);
}
