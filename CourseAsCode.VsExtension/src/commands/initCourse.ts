import type * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import { getWorkspaceRoot } from '../utils/workspace';

const CONFIG_YAML = `CourseName: My Course
ShortName: MC
Summary: ""
`;

const SECTIONS_YAML = `Sections:
  - SectionPath: Sections/Introduction
`;

const INTRO_SECTION_YAML = `SectionName: Introduction
`;

const DEFAULTS_YAML = `# defaults.yaml — course-wide defaults applied to all activity files.
# Values here are overridden by matching fields in individual activity files.
# Remove the leading '#' from any line to activate that default.
#
# Example Quiz defaults:
# Type: QUIZ
# DescriptionFormat: MARKDOWN
# ShuffleQuestions: true
# QuestionBehaviour: Deferred
# TimeExpiryHandling: AutoSubmit
# BuildOnPreviousAttempt: false
`;

export function courseExists(root: string): boolean {
  return (
    fs.existsSync(path.join(root, 'config.yaml')) ||
    fs.existsSync(path.join(root, 'sections.yaml')) ||
    fs.existsSync(path.join(root, 'Sections'))
  );
}

export function scaffoldCourse(root: string): void {
  fs.writeFileSync(path.join(root, 'config.yaml'), CONFIG_YAML, 'utf8');
  fs.writeFileSync(path.join(root, 'sections.yaml'), SECTIONS_YAML, 'utf8');
  fs.writeFileSync(path.join(root, 'defaults.yaml'), DEFAULTS_YAML, 'utf8');
  const introDir = path.join(root, 'Sections', 'Introduction');
  fs.mkdirSync(introDir, { recursive: true });
  fs.writeFileSync(path.join(introDir, 'section.yaml'), INTRO_SECTION_YAML, 'utf8');
}

export async function initCourseCommand(folderUri?: vscode.Uri): Promise<void> {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  let root: string | undefined;

  if (folderUri) {
    root = folderUri.fsPath;
  } else {
    const selected = await vs.window.showOpenDialog({
      canSelectFiles: false,
      canSelectFolders: true,
      canSelectMany: false,
      openLabel: 'Select course root folder',
    });
    if (!selected || selected.length === 0) {
      return; // user cancelled — silent return per spec
    }
    root = selected[0].fsPath;
  }

  if (courseExists(root)) {
    vs.window.showErrorMessage(
      'A course already exists in this folder. Init Course aborted.'
    );
    return;
  }

  scaffoldCourse(root);
  vs.window.showInformationMessage(`Course scaffolded at ${root}`);
}
