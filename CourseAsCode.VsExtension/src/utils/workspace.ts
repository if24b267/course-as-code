import type * as vscode from 'vscode';

/**
 * Returns the fs path of the first workspace folder, or undefined if none is open.
 * All commands use workspaceFolders[0] as the course root.
 */
export function getWorkspaceRoot(): string | undefined {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;
  const folders = vs.workspace.workspaceFolders;
  if (!folders || folders.length === 0) {
    return undefined;
  }
  return folders[0].uri.fsPath;
}
