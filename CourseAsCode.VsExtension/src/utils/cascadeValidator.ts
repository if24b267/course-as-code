import type * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';

export function findCourseRoot(filePath: string): string | null {
  let dir = path.dirname(filePath);
  while (dir) {
    if (fs.existsSync(path.join(dir, 'config.yaml'))) {
      return dir;
    }
    const parentDir = path.dirname(dir);
    if (parentDir === dir) {
      break;
    }
    dir = parentDir;
  }
  return null;
}

export function getParentDefaultsPaths(filePath: string, courseRoot: string): string[] {
  const defaultsPaths: string[] = [];
  const fileName = path.basename(filePath);
  
  // Determine type suffix
  let typeSuffix: string | null = null;
  if (fileName.startsWith('default_')) {
    const match = fileName.match(/^default_([a-z0-9_-]+)\.yaml$/i);
    if (match) typeSuffix = match[1].toLowerCase();
  } else if (fileName.startsWith('Forum_')) {
    typeSuffix = 'forum';
  } else if (fileName.startsWith('Page_') || fileName.startsWith('PGE_')) {
    typeSuffix = 'page';
  } else if (fileName.startsWith('Quiz_') || fileName.startsWith('QUIZ_')) {
    typeSuffix = 'quiz';
  } else if (fileName.startsWith('Assignment_') || fileName.startsWith('ASN_')) {
    typeSuffix = 'assignment';
  } else if (fileName.startsWith('Question_')) {
    typeSuffix = 'question';
  } else {
    const normalizedPath = path.normalize(filePath).toLowerCase();
    const pathParts = normalizedPath.split(/[/\\]/);
    if (pathParts.includes('questionpool') || pathParts.includes('questions')) {
      typeSuffix = 'question';
    }
  }

  // Determine starting directory for parent search
  let startDir = path.dirname(filePath);
  if (path.basename(startDir) === 'defaults' || fileName.startsWith('default')) {
    const parentOfDefaults = path.dirname(startDir);
    startDir = path.dirname(parentOfDefaults);
  }

  let dir = startDir;
  const normalizedCourseRoot = path.normalize(courseRoot).toLowerCase();

  while (dir) {
    const normalizedDir = path.normalize(dir).toLowerCase();
    if (!normalizedDir.startsWith(normalizedCourseRoot)) {
      break;
    }

    const defaultsDir = path.join(dir, 'defaults');
    
    // 1. Specific defaults (e.g. default_quiz.yaml)
    if (typeSuffix) {
      const specificPath = path.join(defaultsDir, `default_${typeSuffix}.yaml`);
      if (fs.existsSync(specificPath) && path.resolve(specificPath) !== path.resolve(filePath)) {
        defaultsPaths.push(specificPath);
      }
    }

    // 2. Generic defaults (default.yaml)
    const genericPath = path.join(defaultsDir, 'default.yaml');
    if (fs.existsSync(genericPath) && path.resolve(genericPath) !== path.resolve(filePath)) {
      defaultsPaths.push(genericPath);
    }

    // Move up
    const parentDir = path.dirname(dir);
    if (parentDir === dir) {
      break;
    }
    dir = parentDir;
  }

  return defaultsPaths;
}

export function getTopLevelKeys(filePath: string): Set<string> {
  const keys = new Set<string>();
  try {
    if (fs.existsSync(filePath)) {
      const content = fs.readFileSync(filePath, 'utf8');
      const lines = content.split(/\r?\n/);
      for (const line of lines) {
        const match = line.match(/^([a-zA-Z0-9_-]+)\s*:/);
        if (match) {
          keys.add(match[1]);
        }
      }
    }
  } catch (err) {
    console.error(`Failed to read keys from ${filePath}:`, err);
  }
  return keys;
}

export function validateDocument(
  document: vscode.TextDocument,
  collection: vscode.DiagnosticCollection
): void {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  if (document.languageId !== 'yaml') {
    return;
  }

  const filePath = document.uri.fsPath;
  const fileName = path.basename(filePath);
  
  // Ignore sections.yaml, section.yaml, and config.yaml as they don't participate in cascading defaults
  if (
    fileName === 'sections.yaml' ||
    fileName === 'section.yaml' ||
    fileName === 'config.yaml'
  ) {
    collection.delete(document.uri);
    return;
  }

  // Find course root
  const courseRoot = findCourseRoot(filePath);
  if (!courseRoot) {
    collection.delete(document.uri);
    return;
  }

  const parentDefaultsPaths = getParentDefaultsPaths(filePath, courseRoot);
  if (parentDefaultsPaths.length === 0) {
    collection.delete(document.uri);
    return;
  }

  const lines = document.getText().split(/\r?\n/);
  const diagnosticsList: vscode.Diagnostic[] = [];

  // Cache parent files keys to avoid reading file multiple times for each line
  const parentKeysCache = new Map<string, { relPath: string; keys: Set<string> }>();
  for (const parentFile of parentDefaultsPaths) {
    const keys = getTopLevelKeys(parentFile);
    const relPath = path.relative(courseRoot, parentFile).replace(/\\/g, '/');
    parentKeysCache.set(parentFile, { relPath, keys });
  }

  for (let i = 0; i < lines.length; i++) {
    const line = lines[i];
    const match = line.match(/^([a-zA-Z0-9_-]+)\s*:/);
    if (match) {
      const key = match[1];
      
      // Skip validation for Type or Name as they are required standard activity fields
      if (key === 'Type' || key === 'Name') {
        continue;
      }

      const overridingFiles: string[] = [];
      
      for (const [_, cache] of parentKeysCache) {
        if (cache.keys.has(key)) {
          overridingFiles.push(cache.relPath);
        }
      }

      if (overridingFiles.length > 0) {
        const keyStart = line.indexOf(key);
        const keyEnd = keyStart + key.length;
        const range = new vs.Range(i, keyStart, i, keyEnd);
        
        const message = `"${key}" overrides a default value defined in: ${overridingFiles.join(', ')}`;
        const diagnostic = new vs.Diagnostic(
          range,
          message,
          vs.DiagnosticSeverity.Hint
        );
        diagnostic.code = 'cascade-override';
        diagnosticsList.push(diagnostic);
      }
    }
  }

  collection.set(document.uri, diagnosticsList);
}
