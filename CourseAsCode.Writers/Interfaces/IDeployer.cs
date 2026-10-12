namespace CourseAsCode.Writers.Interfaces
{
    public interface IDeployer
    {
        /// <summary>
        /// Spielt die erzeugte .mbz-Datei in eine Moodle-Instanz ein.
        /// </summary>
        /// <param name="mbzFilePath">Pfad zur erzeugten .mbz-Datei.</param>
        /// <param name="config">Zielkonfiguration (Container bzw. Host, Kategorie-ID).</param>
        /// <returns>
        /// <para>True, wenn der Kurs erfolgreich eingespielt wurde.</para>
        /// <para>False bei einem Fehler (Details stehen im Log).</para>
        /// </returns>
        public bool Deploy(string mbzFilePath, DeployConfig config);
    }
}
