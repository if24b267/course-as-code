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
            - "mariadb_data:/bitnami/mariadb"
    moodle:
        image: docker.io/bitnamilegacy/moodle:5.0
        ports:
            - "80:8080"
            - "443:8443"
        environment:
            - MOODLE_DATABASE_HOST=mariadb
            - MOODLE_DATABASE_PORT_NUMBER=3306
            - MOODLE_DATABASE_USER=bn_moodle
            - MOODLE_DATABASE_NAME=bitnami_moodle
            # ALLOW_EMPTY_PASSWORD is recommended only for development.
            - ALLOW_EMPTY_PASSWORD=yes
        volumes:
            - "moodle_data:/bitnami/moodle"
            - "moodledata_data:/bitnami/moodledata"
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

## Installing CodeRunner and its dependencies

CodeRunner requires an additional Moodle question behaviour plugin called **Adaptive adapted for CodeRunner**.

Both plugins must be compatible with the installed Moodle version.

### Installation

1. Open Moodle at `http://localhost` and log in as an administrator.
2. Navigate to **Site administration > Plugins > Install plugins**.
3. Download the required plugins:
    - [Adaptive adapted for CodeRunner](https://github.com/trampgeek/moodle-qbehaviour_adaptive_adapted_for_coderunner)
    - [CodeRunner](https://github.com/trampgeek/moodle-qtype_coderunner)
4. Download the ZIP archives from GitHub using **Code > Download ZIP**.
5. Install the Adaptive adapted for CodeRunner plugin first.
6. Complete the Moodle database upgrade when prompted.
7. Install the CodeRunner plugin and complete its database upgrade.
8. Verify that both plugins appear in the installed plugins list.

**Note:** GitHub ZIP archives may require adjustments to their directory structure before Moodle accepts them.

## Restoring a Moodle backup

To restore a generated `.mbz` file:

1. Open **Site administration > Courses > Restore course**.
2. Upload the Moodle backup file.
3. Select **Restore as a new course**.
4. Select a destination course category.
5. Continue through the settings and schema steps.
6. Confirm the restore operation.
7. Verify that the course was restored successfully.

If the restore operation runs asynchronously, Moodle may continue processing it in the background.

## Troubleshooting

### Moodle is not accessible

Check the Docker containers:

```powershell
docker compose ps
```

Inspect the Moodle logs:

```powershell
docker compose logs moodle --tail=100
```

### Plugin installation fails

Check the plugin version and dependencies. Make sure the selected release is compatible with Moodle 5.0.

### Data persistence

Moodle and MariaDB use Docker volumes to persist their data.

Avoid using `docker compose down -v` during normal development, because this removes the persistent volumes and their stored data.
