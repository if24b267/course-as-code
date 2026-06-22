import type * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import { sanitizeName } from '../templates';
import { QUESTION_TYPES, QuestionType, getQuestionTemplate } from '../questionTemplates';
import { getWorkspaceRoot } from '../utils/workspace';

/**
 * Returns the next free QuestionId for a questionpool directory.
 *
 * Each question's QuestionId is written verbatim as the Moodle question-bank `id`
 * (see QuestionsWriter / QuizWriter). If two questions share an id, Moodle treats
 * them as the same question on import and merges their answers — so every question
 * file must carry a unique, non-zero QuestionId. We assign max(existing) + 1.
 */
export function nextQuestionId(questionPoolDir: string): number {
  let max = 0;
  let files: string[] = [];
  try {
    files = fs.readdirSync(questionPoolDir).filter(f => f.endsWith('.yaml'));
  } catch {
    return 1;
  }
  for (const f of files) {
    try {
      const content = fs.readFileSync(path.join(questionPoolDir, f), 'utf8');
      const match = content.match(/^QuestionId:\s*(\d+)/m);
      if (match) {
        const id = parseInt(match[1], 10);
        if (id > max) max = id;
      }
    } catch {
      // unreadable file — skip
    }
  }
  return max + 1;
}

/** Injects a unique `QuestionId` line right after the leading `QuestionName` line. */
export function withQuestionId(template: string, id: number): string {
  if (/^QuestionId:/m.test(template)) {
    return template.replace(/^QuestionId:.*$/m, `QuestionId: ${id}`);
  }
  return template.replace(/^(QuestionName:.*\r?\n)/, `$1QuestionId: ${id}\n`);
}

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

  // Step 1: Question type picker
  const selectedType = await vs.window.showQuickPick(QUESTION_TYPES as string[], {
    placeHolder: 'Select question type',
  }) as QuestionType | undefined;
  if (!selectedType) {
    return;
  }

  // Step 2: Filename input
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

  const id = nextQuestionId(questionPoolDir);
  const content = withQuestionId(getQuestionTemplate(selectedType), id);
  fs.writeFileSync(questionFilePath, content, 'utf8');

  const doc = await vs.workspace.openTextDocument(questionFilePath);
  await vs.window.showTextDocument(doc);
  vs.window.showInformationMessage(`Created ${selectedType} question ${fileName} (QuestionId ${id}) in questionpool`);
}
