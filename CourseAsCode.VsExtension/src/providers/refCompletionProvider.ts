import type * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';

export function getQuestionPoolCompletions(quizFsPath: string): string[] {
  try {
    let dir = path.dirname(quizFsPath);
    while (true) {
      const candidate = path.join(dir, 'questionpool');
      if (fs.existsSync(candidate) && fs.statSync(candidate).isDirectory()) {
        const quizDir = path.dirname(quizFsPath);
        return fs.readdirSync(candidate)
          .filter(f => f.endsWith('.yaml'))
          .map(f => {
            const absNoExt = path.join(candidate, f.slice(0, -5));
            return path.relative(quizDir, absNoExt).replace(/\\/g, '/');
          });
      }
      const parent = path.dirname(dir);
      if (parent === dir) break;
      dir = parent;
    }
    return [];
  } catch {
    return [];
  }
}

/** Returns bare filenames (e.g. "Quiz_test.yaml") — caller prepends "Activities/" for insertText. */
export function getActivityCompletions(sectionYamlFsPath: string): string[] {
  try {
    const activitiesDir = path.join(path.dirname(sectionYamlFsPath), 'Activities');
    if (!fs.existsSync(activitiesDir)) return [];
    return fs.readdirSync(activitiesDir).filter(f => f.endsWith('.yaml'));
  } catch {
    return [];
  }
}

export function getSectionCompletions(sectionsYamlFsPath: string): string[] {
  try {
    const sectionsDir = path.join(path.dirname(sectionsYamlFsPath), 'Sections');
    if (!fs.existsSync(sectionsDir)) return [];
    return fs.readdirSync(sectionsDir).filter(name =>
      fs.statSync(path.join(sectionsDir, name)).isDirectory()
    );
  } catch {
    return [];
  }
}

export class RefCompletionProvider implements vscode.CompletionItemProvider {
  provideCompletionItems(
    document: vscode.TextDocument,
    position: vscode.Position
  ): vscode.CompletionItem[] {
    const fileName = document.fileName;
    const lineText = document.lineAt(position.line).text;
    const basename = path.basename(fileName);

    try {
      // eslint-disable-next-line @typescript-eslint/no-require-imports
      const vs = require('vscode') as typeof vscode;
      if (/^quiz_.*\.yaml$/i.test(basename) && /^\s*-\s+Ref:/.test(lineText)) {
        return getQuestionPoolCompletions(fileName).map(ref => {
          // insertText matches label for Ref — kept explicit for consistency with the other cases
          const item = new vs.CompletionItem(ref, vs.CompletionItemKind.File);
          item.insertText = ref;
          return item;
        });
      }
      if (basename === 'section.yaml' && /^\s*ActivityFilePath:/.test(lineText)) {
        return getActivityCompletions(fileName).map(name => {
          const item = new vs.CompletionItem(name, vs.CompletionItemKind.File);
          item.insertText = `Activities/${name}`;
          return item;
        });
      }
      if (basename === 'sections.yaml' && /^\s*SectionPath:/.test(lineText)) {
        return getSectionCompletions(fileName).map(name => {
          const item = new vs.CompletionItem(name, vs.CompletionItemKind.Folder);
          item.insertText = `Sections/${name}`;
          return item;
        });
      }
    } catch {
      return [];
    }
    return [];
  }
}
