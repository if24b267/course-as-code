# Setting up a local Moodle Environment
First, we need a local Moodle instance that you can use to test and preview your exported courses. We use Docker for this with an image from Bitnami [[hub.docker.com](https://hub.docker.com/r/bitnami/moodle)].

To set up a local installation of Moodle for development, follow these instructions:

1. Create a new file: `docker-compose.yml`
2. Add the following content to the file:
```yaml
# Copyright Broadcom, Inc. All Rights Reserved.
# SPDX-License-Identifier: APACHE-2.0

services:
  mariadb:
    image: docker.io/bitnamilegacy/mariadb:latest
    environment:
      # ALLOW_EMPTY_PASSWORD is recommended only for development.
      - ALLOW_EMPTY_PASSWORD=yes
      - MARIADB_USER=bn_moodle
      - MARIADB_DATABASE=bitnami_moodle
      - MARIADB_CHARACTER_SET=utf8mb4
      - MARIADB_COLLATE=utf8mb4_unicode_ci
    volumes:
      - 'mariadb_data:/bitnami/mariadb'
  moodle:
    image: docker.io/bitnamilegacy/moodle:5.0
    ports:
      - '80:8080'
      - '443:8443'
    environment:
      - MOODLE_DATABASE_HOST=mariadb
      - MOODLE_DATABASE_PORT_NUMBER=3306
      - MOODLE_DATABASE_USER=bn_moodle
      - MOODLE_DATABASE_NAME=bitnami_moodle
      # ALLOW_EMPTY_PASSWORD is recommended only for development.
      - ALLOW_EMPTY_PASSWORD=yes
    volumes:
      - 'moodle_data:/bitnami/moodle'
      - 'moodledata_data:/bitnami/moodledata'
    depends_on:
      - mariadb

volumes:
  mariadb_data:
    driver: local
  moodle_data:
    driver: local
  moodledata_data:
    driver: local
```
3. Start the containers:
```bash
(sudo) docker compose up -d
```
4. Moodle is now reachable at [`http://localhost`](http://localhost)
5. Log in with the default credentials:
```txt
user: user
password: bitnami
```
6. Go to **Site Administration** > **Courses** > **Backups** > **Asynchronous backup/restore** and disable `Enable asynchronous backups`.
7. Go to **Site Administration** > **Plugins** > **Install plugins** and install the [**CodeRunner** plugin](https://moodle.org/plugins/qtype_coderunner).