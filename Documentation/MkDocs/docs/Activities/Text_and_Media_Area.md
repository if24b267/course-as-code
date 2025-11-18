# [Text and Media Area](https://docs.moodle.org/500/en/Text_and_media_area)

The **Text and Media Area** (formerly: Label) can be used to embed formatted content (text, photos, multimedia, code, etc.) into a Moodle course, allowing for the design of nice-looking sections. The content can be formatted as `HTML`, `PlainText`, or `Markdown`.

Therefore, to define a Text and Media Area, only the content to be displayed, in the form of a **string** (`Content`), and the **format** in which the content is provided (`FormattingFormat`) are necessary.

!!! info
    `FormattingFormat` is an **enum** that can take the values `HTML`, `PlainText`, `Markdown`, and `MoodleAutoFormat`.