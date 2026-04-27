import * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import type * as yamlTypes from 'js-yaml';
import { getWorkspaceRoot } from '../utils/workspace';

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

type CourseNode = SectionItem | ActivityItem;

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
      return this._getSections(root);
    }

    if (element instanceof SectionItem) {
      return this._getActivities(element.sectionDir);
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

    return entries.map(entry => {
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

  private _getActivities(sectionDir: string): ActivityItem[] {
    const activitiesDir = path.join(sectionDir, 'Activities');
    if (!fs.existsSync(activitiesDir)) return [];

    return fs.readdirSync(activitiesDir)
      .filter(f => f.endsWith('.yaml'))
      .sort()
      .map(f => new ActivityItem(f, path.join(activitiesDir, f)));
  }
}
