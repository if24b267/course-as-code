### 🏗️ Architecture of Course as Code

**Course as Code** is a modular application that, broadly speaking, consists of three main components:

  * A **Reader component** that reads text files (YAML, Markdown) and stores the information in an internal data model.
  * An **internal data model** that uses classes to represent a Moodle course along with all its supported components.
  * A **Writer component** that subsequently writes the information stored in the internal data model into the `.xml` files required by Moodle.

![Overview the general architecture of Course as Code](https://github.com/Davimalu/course-as-code/blob/develop/Documentation/ArchitectureOverview.png?raw=true) 

For more details, see the [Internal Data Model documentation](https://davimalu.github.io/course-as-code/Architecture/DataModel/) or the [Architecture Overview](https://davimalu.github.io/course-as-code/Architecture/Overview/).