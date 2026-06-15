import * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import type * as yamlTypes from 'js-yaml';
import { getWorkspaceRoot } from '../utils/workspace';
import { listReleases } from '../utils/releaseName';

export class SectionItem extends vscode.TreeItem {
  constructor(
    label: string,
    public readonly sectionDir: string
  ) {
    super(label, vscode.TreeItemCollapsibleState.Collapsed);
    this.contextValue = 'section';
    this.iconPath = new vscode.ThemeIcon('folder');
    this.command = {
      command: 'vscode.open',
      title: 'Open',
      arguments: [vscode.Uri.file(path.join(sectionDir, 'section.yaml'))],
    };
  }
}

export class ActivityItem extends vscode.TreeItem {
  constructor(label: string, filePath: string) {
    super(label, vscode.TreeItemCollapsibleState.None);
    this.contextValue = 'activity';
    this.iconPath = new vscode.ThemeIcon('file');
    this.command = {
      command: 'vscode.open',
      title: 'Open',
      arguments: [vscode.Uri.file(filePath)],
    };
  }
}

export class ReleasesRootItem extends vscode.TreeItem {
  constructor(public readonly outputDir: string) {
    super('Releases', vscode.TreeItemCollapsibleState.Collapsed);
    this.contextValue = 'releases';
    this.iconPath = new vscode.ThemeIcon('archive');
  }
}

export class ReleaseItem extends vscode.TreeItem {
  constructor(mbzPath: string) {
    super(path.basename(mbzPath), vscode.TreeItemCollapsibleState.None);
    this.contextValue = 'release';
    this.resourceUri = vscode.Uri.file(mbzPath);
    this.iconPath = new vscode.ThemeIcon('file-zip');
    this.command = {
      command: 'courseascodetools.revealRelease',
      title: 'Reveal in File Explorer',
      arguments: [this],
    };
  }
}

type CourseNode = SectionItem | ActivityItem | ReleasesRootItem | ReleaseItem;

export class CourseExplorerProvider implements vscode.TreeDataProvider<CourseNode> {
  private readonly _onDidChangeTreeData =
    new vscode.EventEmitter<CourseNode | undefined | void>();
  readonly onDidChangeTreeData = this._onDidChangeTreeData.event;

  refresh(): void {
    this._onDidChangeTreeData.fire();
  }

  getTreeItem(element: CourseNode): vscode.TreeItem {
    return element;
  }

  getChildren(element?: CourseNode): CourseNode[] {
    const root = getWorkspaceRoot();
    if (!root) return [];

    if (!element) {
      const nodes: CourseNode[] = [...this._getSections(root)];
      nodes.push(new ReleasesRootItem(path.join(root, 'output')));
      return nodes;
    }

    if (element instanceof SectionItem) {
      return this._getActivities(element.sectionDir);
    }

    if (element instanceof ReleasesRootItem) {
      return this._getReleases(element.outputDir);
    }

    return [];
  }

  private _getSections(root: string): SectionItem[] {
    // eslint-disable-next-line @typescript-eslint/no-require-imports
    const yaml = require('js-yaml') as typeof yamlTypes;
    const sectionsFile = path.join(root, 'sections.yaml');
    if (!fs.existsSync(sectionsFile)) return [];

    let entries: Array<{ SectionPath: string }> = [];
    try {
      const parsed = yaml.load(fs.readFileSync(sectionsFile, 'utf8')) as {
        Sections?: Array<{ SectionPath: string }>;
      } | null;
      entries = parsed?.Sections ?? [];
    } catch (err) {
      console.error(`CourseExplorer: failed to parse sections.yaml: ${err instanceof Error ? err.message : String(err)}`);
      return [];
    }

    return entries
      .filter(entry => typeof entry?.SectionPath === 'string' && entry.SectionPath.length > 0)
      .map(entry => {
        const absDir = path.join(root, entry.SectionPath);
        let label = path.basename(entry.SectionPath);
        const sectionYamlPath = path.join(absDir, 'section.yaml');
        if (fs.existsSync(sectionYamlPath)) {
          try {
            const p = yaml.load(fs.readFileSync(sectionYamlPath, 'utf8')) as {
              SectionName?: string;
            } | null;
            if (p?.SectionName) label = p.SectionName;
          } catch (err) {
            console.warn(`CourseExplorer: failed to parse section.yaml in ${absDir}: ${err instanceof Error ? err.message : String(err)}`);
          }
        }
        return new SectionItem(label, absDir);
      });
  }

  private _getReleases(outputDir: string): ReleaseItem[] {
    return listReleases(outputDir)
      .map(p => {
        let mtime = 0;
        try {
          mtime = fs.statSync(p).mtimeMs;
        } catch {
          // file vanished between readdir and stat — sort it last
        }
        return { p, mtime };
      })
      .sort((a, b) => b.mtime - a.mtime) // newest first
      .map(({ p }) => new ReleaseItem(p));
  }

  private _getActivities(sectionDir: string): ActivityItem[] {
    // eslint-disable-next-line @typescript-eslint/no-require-imports
    const yaml = require('js-yaml') as typeof yamlTypes;
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');
    if (!fs.existsSync(sectionYamlPath)) return [];

    let entries: Array<{ ActivityFilePath: string }> = [];
    try {
      const parsed = yaml.load(fs.readFileSync(sectionYamlPath, 'utf8')) as {
        Activities?: Array<{ ActivityFilePath: string }>;
      } | null;
      entries = parsed?.Activities ?? [];
    } catch {
      return [];
    }

    return entries
      .filter(e => typeof e?.ActivityFilePath === 'string' && e.ActivityFilePath.length > 0)
      .map(e => {
        const absPath = path.resolve(sectionDir, e.ActivityFilePath);
        const label = path.basename(e.ActivityFilePath);
        if (fs.existsSync(absPath)) {
          return new ActivityItem(label, absPath);
        }
        const item = new ActivityItem(`${label} (missing)`, absPath);
        item.iconPath = new vscode.ThemeIcon('warning');
        item.command = undefined;
        return item;
      });
  }
}
