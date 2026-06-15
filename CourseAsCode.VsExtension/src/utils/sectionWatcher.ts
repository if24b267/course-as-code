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
  try {
    return fs.readdirSync(sectionsDir).some(
      name => fs.statSync(path.join(sectionsDir, name)).isDirectory()
    );
  } catch {
    return false;
  }
}

function updateContext(): void {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;
  try {
    vs.commands.executeCommand(
      'setContext',
      'courseascodetools.hasSections',
      hasSectionsOnDisk()
    );
  } catch (err) {
    console.error(
      `CourseAsCode: failed to update context — ${err instanceof Error ? err.message : String(err)}`
    );
  }
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

  let watcher: vscode.FileSystemWatcher;
  try {
    watcher = vs.workspace.createFileSystemWatcher(
      new vs.RelativePattern(root, '{sections.yaml,Sections/**}'),
      false, // onDidCreate fires
      false, // onDidChange fires (catches sections.yaml edits)
      false  // onDidDelete fires
    );
  } catch (err) {
    console.error(
      `CourseAsCode: failed to create FileSystemWatcher — ${err instanceof Error ? err.message : String(err)}`
    );
    return;
  }

  const handler = (uri: vscode.Uri) => {
    try {
      // If a section folder was deleted, check it actually no longer exists
      // before refreshing — avoids spurious errors on temp file events
      if (!fs.existsSync(uri.fsPath) || fs.existsSync(uri.fsPath)) {
        updateContext();
        onRefresh?.();
      }
    } catch (err) {
      console.warn(
        `CourseAsCode: watcher handler error for ${uri.fsPath} — ${err instanceof Error ? err.message : String(err)}`
      );
      // Still refresh so the Explorer stays in sync
      updateContext();
      onRefresh?.();
    }
  };

  watcher.onDidCreate(handler);
  watcher.onDidChange(handler);
  watcher.onDidDelete(handler);

  context.subscriptions.push(watcher);
}