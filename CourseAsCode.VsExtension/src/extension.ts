import * as vscode from 'vscode';
import { initCourseCommand } from './commands/initCourse';
import { updateSectionsCommand } from './commands/updateSections';
import { createActivityCommand } from './commands/createActivity';
import { createSectionCommand } from './commands/createSection';
import { createQuestionCommand } from './commands/createQuestion';
import { CourseExplorerProvider } from './providers/courseExplorerProvider';
import { registerSectionWatcher } from './utils/sectionWatcher';
import { validateDocument } from './utils/cascadeValidator';
import { RefCompletionProvider } from './providers/refCompletionProvider';

export function activate(context: vscode.ExtensionContext): void {
  const provider = new CourseExplorerProvider();
  vscode.window.registerTreeDataProvider('courseascodetools.explorer', provider);

  registerSectionWatcher(context, () => provider.refresh());

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

  // Validate all currently open documents
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
  );
}

export function deactivate(): void {}
