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

const DEFAULT_ASSIGNMENT_YAML = `AssignmentTextFormat: HTML
AlwaysShowDescription: true
AllowSubmissionDrafts: false
SendNotifications: false
SendLateNotifications: false
SendStudentNotifications: false
CompletionSubmit: true
RequireSubmissionStatement: false
AllowTeamSubmissions: false
RequireAllTeamMembersSubmit: false
BlindMarking: false
HideGrader: false
RevealIdentities: false
AttemptReopenMethod: none
MaxAttempts: 1
MarkingWorkflow: false
MarkingAllocation: false
MarkingAnonymous: false
PreventSubmissionNotInGroup: false
GradingLocked: false
`;

const DEFAULT_FORUM_YAML = `ForumType: General
IntroFormat: 1
DueDate: 0
CutoffDate: 0
Assessed: false
AssessTimeStart: 0
AssessTimeFinish: 0
Scale: 0
MaxBytes: 512000
MaxAttachments: 1
SubscriptionMode: Optional
TrackingType: Optional
RssType: None
RssArticles: 0
WarnAfter: 0
BlockAfter: 0
BlockPeriod: 0
CompletionDiscussions: 0
CompletionReplies: 0
CompletionPosts: 0
DisplayWordCount: false
LockDiscussionAfter: 0
GradeForum: false
`;

const DEFAULT_QUESTION_YAML = `DefaultMark: 1
QuestionReady: true
`;

const DEFAULT_QUIZ_YAML = `Type: QUIZ
DescriptionFormat: MARKDOWN
OpeningTime: 31-01-2023 00:00
ClosingTime: 30-12-2025 23:59
TimeExpiryHandling: AutoSubmit
BuildOnPreviousAttempt: false
QuestionBehaviour: Deferred
ShuffleQuestions: true
AllowRedosDuringAttempt: false
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

  // Create defaults directory and default files
  const defaultsDir = path.join(root, 'defaults');
  fs.mkdirSync(defaultsDir, { recursive: true });
  fs.writeFileSync(path.join(defaultsDir, 'default_assignment.yaml'), DEFAULT_ASSIGNMENT_YAML, 'utf8');
  fs.writeFileSync(path.join(defaultsDir, 'default_forum.yaml'), DEFAULT_FORUM_YAML, 'utf8');
  fs.writeFileSync(path.join(defaultsDir, 'default_question.yaml'), DEFAULT_QUESTION_YAML, 'utf8');
  fs.writeFileSync(path.join(defaultsDir, 'default_quiz.yaml'), DEFAULT_QUIZ_YAML, 'utf8');

  // Create empty questionpool directory
  const questionPoolDir = path.join(root, 'questionpool');
  fs.mkdirSync(questionPoolDir, { recursive: true });

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
