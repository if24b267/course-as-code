import * as vscode from 'vscode';
import { initCourseCommand } from './commands/initCourse';
import { updateSectionsCommand } from './commands/updateSections';
import { createActivityCommand } from './commands/createActivity';
import { registerSectionWatcher } from './utils/sectionWatcher';

export function activate(context: vscode.ExtensionContext): void {
  registerSectionWatcher(context);

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
    )
  );
}

export function deactivate(): void {}
