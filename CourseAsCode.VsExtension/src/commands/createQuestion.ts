import type * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import { sanitizeName } from '../templates';
import { getWorkspaceRoot } from '../utils/workspace';

const QUESTION_TEMPLATE = `QuestionName: ""
Category: ""
QuestionId: 0
QuestionText: ""
QuestionTextFormat: MARKDOWN
QuestionType: SINGLECHOICE
Answers:
  - AnswerText: ""
    AnswerTextFormat: MARKDOWN
    AnswerGrade: 1
    Feedback: ""
`;

export async function createQuestionCommand(): Promise<void> {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  const root = getWorkspaceRoot();
  if (!root) {
    vs.window.showErrorMessage('No workspace folder open.');
    return;
  }

  const questionPoolDir = path.join(root, 'questionpool');
  if (!fs.existsSync(questionPoolDir)) {
    fs.mkdirSync(questionPoolDir, { recursive: true });
  }

  const rawName = await vs.window.showInputBox({
    prompt: 'Enter a filename for the new question (without .yaml extension)',
    placeHolder: 'e.g. sample-question',
  });
  if (rawName === undefined) {
    return;
  }

  const sanitized = sanitizeName(rawName.trim());
  if (!sanitized) {
    vs.window.showErrorMessage(
      'Question name is empty after sanitization. Use letters, digits, hyphens, or underscores.'
    );
    return;
  }

  const fileName = `${sanitized}.yaml`;
  const questionFilePath = path.join(questionPoolDir, fileName);

  if (fs.existsSync(questionFilePath)) {
    vs.window.showErrorMessage(`Question file already exists: ${fileName}`);
    return;
  }

  fs.writeFileSync(questionFilePath, QUESTION_TEMPLATE, 'utf8');

  const doc = await vs.workspace.openTextDocument(questionFilePath);
  await vs.window.showTextDocument(doc);
  vs.window.showInformationMessage(`Created question ${fileName} in questionpool`);
}
