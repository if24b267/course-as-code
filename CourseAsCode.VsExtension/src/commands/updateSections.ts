import type * as vscode from 'vscode';
import type * as yamlTypes from 'js-yaml';
import * as fs from 'fs';
import * as path from 'path';
import { getWorkspaceRoot } from '../utils/workspace';

export type ValidationResult = {
  type: 'error' | 'warning' | 'success';
  message: string;
};

interface SectionsYaml {
  Sections?: Array<{ SectionPath: string }>;
}

function sectionNameFromPath(sectionPath: string): string {
  return path.basename(sectionPath);
}

export function scaffoldMissingSection(root: string, sectionPath: string, displayName?: string): void {
  const absDir = path.join(root, sectionPath);
  const sectionName = displayName ?? sectionNameFromPath(sectionPath);

  fs.mkdirSync(absDir, { recursive: true });
  fs.mkdirSync(path.join(absDir, 'Activities'), { recursive: true });
  fs.writeFileSync(
    path.join(absDir, 'section.yaml'),
    `SectionName: ${sectionName}\n`,
    'utf8'
  );
}

export function validateSections(root: string): ValidationResult[] {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;

  const sectionsFile = path.join(root, 'sections.yaml');

  if (!fs.existsSync(sectionsFile)) {
    return [{
      type: 'error',
      message: 'No sections.yaml found in workspace root. Run Init Course first.',
    }];
  }

  const raw = fs.readFileSync(sectionsFile, 'utf8');
  const parsed = yaml.load(raw) as SectionsYaml | null;
  const sections = parsed?.Sections ?? [];

  const results: ValidationResult[] = [];

  for (const entry of sections) {
    const sectionPath = entry.SectionPath;
    const absDir = path.join(root, sectionPath);

    if (!fs.existsSync(absDir)) {
      results.push({ type: 'warning', message: `Missing directory: ${sectionPath}` });
    } else if (!fs.existsSync(path.join(absDir, 'section.yaml'))) {
      results.push({ type: 'warning', message: `Missing section.yaml in: ${sectionPath}` });
    }
  }

  if (results.length === 0) {
    results.push({ type: 'success', message: 'All sections are valid.' });
  }

  return results;
}

export function getMissingSections(root: string): string[] {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;

  const sectionsFile = path.join(root, 'sections.yaml');
  if (!fs.existsSync(sectionsFile)) {
    return [];
  }

  const raw = fs.readFileSync(sectionsFile, 'utf8');
  const parsed = yaml.load(raw) as SectionsYaml | null;
  const sections = parsed?.Sections ?? [];

  return sections
    .map(e => e.SectionPath)
    .filter(p => !fs.existsSync(path.join(root, p)));
}

export async function updateSectionsCommand(): Promise<void> {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  const root = getWorkspaceRoot();
  if (!root) {
    vs.window.showErrorMessage('No workspace folder open.');
    return;
  }

  const missing = getMissingSections(root);

  if (missing.length > 0) {
    const answer = await vs.window.showWarningMessage(
      `${missing.length} section(s) missing on disk. Create them now?`,
      'Create',
      'Cancel'
    );

    if (answer !== 'Create') {
      return;
    }

    for (const sectionPath of missing) {
      scaffoldMissingSection(root, sectionPath);
    }

    vs.window.showInformationMessage(
      `Created ${missing.length} section(s): ${missing.map(p => path.basename(p)).join(', ')}`
    );
    return;
  }

  const results = validateSections(root);

  for (const result of results) {
    if (result.type === 'error') {
      vs.window.showErrorMessage(result.message);
    } else if (result.type === 'warning') {
      vs.window.showWarningMessage(result.message);
    } else {
      vs.window.showInformationMessage(result.message);
    }
  }
}
