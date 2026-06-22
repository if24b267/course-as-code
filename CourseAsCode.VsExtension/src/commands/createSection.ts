import type * as vscode from 'vscode';
import type * as yamlTypes from 'js-yaml';
import * as fs from 'fs';
import * as path from 'path';
import { sanitizeName } from '../templates';
import { scaffoldMissingSection } from './updateSections';
import { getWorkspaceRoot } from '../utils/workspace';

export function buildSectionPath(rawName: string): string {
  return `Sections/${sanitizeName(rawName)}`;
}

export function appendSectionEntry(sectionsFilePath: string, sectionPath: string): void {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;
  const raw = fs.readFileSync(sectionsFilePath, 'utf8');
  const parsed = (yaml.load(raw) ?? {}) as Record<string, unknown>;
  const sections = (parsed['Sections'] as Array<{ SectionPath: string }> | undefined) ?? [];
  sections.push({ SectionPath: sectionPath });
  parsed['Sections'] = sections;
  fs.writeFileSync(sectionsFilePath, yaml.dump(parsed), 'utf8');
}

export async function createSectionCommand(): Promise<void> {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  const root = getWorkspaceRoot();
  if (!root) {
    vs.window.showErrorMessage('No workspace folder open.');
    return;
  }

  const sectionsFile = path.join(root, 'sections.yaml');
  if (!fs.existsSync(sectionsFile)) {
    vs.window.showErrorMessage('No sections.yaml found. Run Init Course first.');
    return;
  }

  const rawName = await vs.window.showInputBox({
    prompt: 'Enter section name',
    placeHolder: 'e.g. Week 3 - Loops',
  });
  if (rawName === undefined) return;

  const trimmed = rawName.trim();
  const sanitized = sanitizeName(trimmed);
  if (!sanitized || /^[-_]+$/.test(sanitized)) {
    vs.window.showErrorMessage(
      'Section name is empty after sanitization. Use letters, digits, hyphens, or underscores.'
    );
    return;
  }

  const sectionPath = buildSectionPath(trimmed);
  const absDir = path.join(root, sectionPath);

  if (fs.existsSync(absDir)) {
    vs.window.showErrorMessage(`Section already exists: ${sectionPath}`);
    return;
  }

  try {
    appendSectionEntry(sectionsFile, sectionPath);
    scaffoldMissingSection(root, sectionPath, trimmed);
  } catch (err) {
    vs.window.showErrorMessage(
      `Failed to create section: ${err instanceof Error ? err.message : String(err)}`
    );
    return;
  }

  vs.window.showInformationMessage(`Created section: ${trimmed}`);
}
