import * as vscode from 'vscode';
import { initCourseCommand } from './commands/initCourse';
import { updateSectionsCommand } from './commands/updateSections';
import { createActivityCommand } from './commands/createActivity';
import { createSectionCommand } from './commands/createSection';
import { CourseExplorerProvider } from './providers/courseExplorerProvider';
import { registerSectionWatcher } from './utils/sectionWatcher';

export function activate(context: vscode.ExtensionContext): void {
  const provider = new CourseExplorerProvider();
  vscode.window.registerTreeDataProvider('courseascodetools.explorer', provider);

  registerSectionWatcher(context, () => provider.refresh());

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
      'courseascodetools.refreshExplorer',
      () => provider.refresh()
    ),
  );
}

export function deactivate(): void {}
