import type * as vscode from 'vscode';
import type * as yamlTypes from 'js-yaml';
import * as fs from 'fs';
import * as path from 'path';
import { getWorkspaceRoot } from './workspace';

type Finding = {
  line: number;
  message: string;
  severity: 'error' | 'warning';
};

function findLineIndex(lines: string[], searchStr: string): number {
  for (let i = 0; i < lines.length; i++) {
    if (lines[i].includes(searchStr)) return i;
  }
  return 0;
}

export function checkSectionActivities(docFsPath: string, content: string): Finding[] {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;
  let parsed: { Activities?: Array<{ ActivityFilePath?: string }> } | null;
  try {
    parsed = yaml.load(content) as typeof parsed;
  } catch {
    return [];
  }
  if (!parsed?.Activities) return [];

  const sectionDir = path.dirname(docFsPath);
  const lines = content.split('\n');
  const findings: Finding[] = [];

  for (const entry of parsed.Activities) {
    const fp = entry?.ActivityFilePath;
    if (typeof fp !== 'string' || !fp) continue;
    const absPath = path.resolve(sectionDir, fp);
    if (!fs.existsSync(absPath)) {
      findings.push({
        line: findLineIndex(lines, fp),
        message: `Activity file not found: ${fp}`,
        severity: 'error',
      });
    }
  }

  return findings;
}

export function checkQuizRefs(docFsPath: string, content: string): Finding[] {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;
  let parsed: { Questions?: Array<{ Ref?: string }> } | null;
  try {
    parsed = yaml.load(content) as typeof parsed;
  } catch {
    return [];
  }
  if (!parsed?.Questions) return [];

  const quizDir = path.dirname(docFsPath);
  const lines = content.split('\n');
  const findings: Finding[] = [];

  for (const entry of parsed.Questions) {
    const ref = entry?.Ref;
    if (typeof ref !== 'string' || !ref) continue;
    const absPath = path.resolve(quizDir, ref + '.yaml');

    if (!fs.existsSync(absPath)) {
      findings.push({
        line: findLineIndex(lines, ref),
        message: `Question file not found: ${ref}`,
        severity: 'error',
      });
    } else {
      let questionParsed: Record<string, unknown> | null = null;
      try {
        questionParsed = yaml.load(fs.readFileSync(absPath, 'utf8')) as Record<string, unknown> | null;
      } catch {
        // unreadable question file — treat as warning
      }
      if (!questionParsed || !('QuestionId' in questionParsed)) {
        findings.push({
          line: findLineIndex(lines, ref),
          message: `Question file has no QuestionId: ${ref}`,
          severity: 'warning',
        });
      }
    }
  }

  return findings;
}

export function validateDocument(
  doc: vscode.TextDocument,
  diagnostics: vscode.DiagnosticCollection
): void {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  if (!doc.fileName.endsWith('.yaml')) {
    diagnostics.delete(doc.uri);
    return;
  }

  const root = getWorkspaceRoot();
  if (!root || !doc.fileName.startsWith(root)) {
    diagnostics.delete(doc.uri);
    return;
  }

  const content = doc.getText();
  const lines = content.split('\n');
  const basename = path.basename(doc.fileName);

  let findings: Finding[] = [];
  try {
    if (basename === 'section.yaml') {
      findings = checkSectionActivities(doc.fileName, content);
    } else if (/^Quiz_.*\.yaml$/.test(basename)) {
      findings = checkQuizRefs(doc.fileName, content);
    } else {
      diagnostics.delete(doc.uri);
      return;
    }
  } catch {
    diagnostics.delete(doc.uri);
    return;
  }

  const diags = findings.map(f => {
    const lineText = lines[f.line] ?? '';
    const range = new vs.Range(f.line, 0, f.line, lineText.length);
    const severity =
      f.severity === 'error'
        ? vs.DiagnosticSeverity.Error
        : vs.DiagnosticSeverity.Warning;
    return new vs.Diagnostic(range, f.message, severity);
  });

  diagnostics.set(doc.uri, diags);
}
