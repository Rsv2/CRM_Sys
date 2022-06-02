namespace Desktop
{
    /// <summary>
    /// ����� ������ �������� �������� ����.
    /// </summary>
    public class DesktopModel
    {
        /// <summary>
        /// ������� ���� � �������
        /// </summary>
        public MainWindow MainWin { get; set; }
        /// <summary>
        /// ����������� ������ �������� �������� ����.
        /// </summary>
        /// <param name="mainWin">������� ���� � �������</param>
        public DesktopModel(MainWindow mainWin)
        {
            MainWin = mainWin;
        }
    }
}



