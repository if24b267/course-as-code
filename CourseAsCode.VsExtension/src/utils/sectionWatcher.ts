import type * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import { getWorkspaceRoot } from './workspace';

function hasSectionsOnDisk(): boolean {
  const root = getWorkspaceRoot();
  if (!root) {
    return false;
  }
  const sectionsDir = path.join(root, 'Sections');
  if (!fs.existsSync(sectionsDir)) {
    return false;
  }
  return fs.readdirSync(sectionsDir).some(
    name => fs.statSync(path.join(sectionsDir, name)).isDirectory()
  );
}

function updateContext(): void {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;
  vs.commands.executeCommand(
    'setContext',
    'courseascodetools.hasSections',
    hasSectionsOnDisk()
  );
}

export function registerSectionWatcher(
  context: vscode.ExtensionContext,
  onRefresh?: () => void
): void {
  updateContext(); // set initial state on activation

  const root = getWorkspaceRoot();
  if (!root) {
    return;
  }

  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;
  const watcher = vs.workspace.createFileSystemWatcher(
    new vs.RelativePattern(root, '{sections.yaml,Sections/**}'),
    false,  // onDidCreate fires
    false,  // onDidChange fires (catches sections.yaml edits)
    false   // onDidDelete fires
  );

  const handler = () => { updateContext(); onRefresh?.(); };
  watcher.onDidCreate(handler);
  watcher.onDidChange(handler);
  watcher.onDidDelete(handler);

  context.subscriptions.push(watcher);
}
