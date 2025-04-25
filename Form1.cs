//VRCMidiTransgerSystem（VMTS）Ver1.0
//Copyright © 2025 スパイル (spire) All Rights Reserved.
//本ソフトウェアはMITライセンスの元提供されています。
//https://opensource.org/license/mit

using Rug.Osc;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;

namespace VRCMidiTransfarSystem
{
    public partial class VRChatMidiTransferSystem : Form
    {
        #region DllImport
        [DllImport("winmm.dll")]
        private static extern int midiInGetNumDevs();

        [DllImport("winmm.dll", EntryPoint = "midiInGetDevCaps", CharSet = CharSet.Ansi)]
        extern static MMResult midiInGetDevCaps(uint devId, ref MidiInCaps devCaps, uint devcapsSize);

        [DllImport("winmm.dll")]
        private static extern MMResult midiInOpen(ref IntPtr hMidiIn, uint uDeviceID, MidiInProc dwCallback, uint dwInstance, uint dwFlags);

        [DllImport("winmm.dll")]
        private static extern MMResult midiInStart(IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        private static extern MMResult midiInStop(IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        private static extern MMResult midiInClose(IntPtr hMidiIn);
        #endregion

        #region 列挙体
        public enum MMResult
        {
            NoError = 0,
            UnspecError = 1,
            BadDeciveID = 2,
            NotEnabled = 3,
            DeviceAlreadyAlllocated = 4,
            InvalidHandle = 5,
            NoDriver = 6,
            NoMem = 7,
            NotSupported = 8,
            BadErrNum = 9,
            InvalidFlag = 10,
            InvalidParam = 11,
            HandleBusy = 12,
            InvalidAlias = 13,
            BadDB = 14,
            KeyNotFound = 15,
            ReadError = 16,
            WriteError = 17,
            DeleteError = 18,
            RegistryValueNotFound = 19,
            LastError = 20,
            MoreDate = 21
        }

        public enum MidiCallbackFlags
        {
            NoCallBack = 0,
            Window = 0X10000,
            Thread = 0X20000,
            Function = 0X30000,
            CallBackEvent = 0X50000,
            MidiIOStatus = 0X20

        }

        public enum MidiMsgTypes
        {
            MM_MIM_OPEN = 961,
            MM_MIM_CLOSE = 962,
            MM_MIM_DATA = 963,
            MM_MIM_LONGDATA = 964,
            MM_MIM_ERROR = 965,
            MM_MIM_LONGERROR = 966,
            MM_MIM_MOREDATA = 972

        }
        #endregion

        #region 構造体
        [StructLayout(LayoutKind.Sequential)]
        public struct MidiInCaps
        {
            public ushort manufacturerId;
            public ushort productId;
            public uint driverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string deviceName;
            public uint support;
        }
        #endregion

        #region 変数
        private IntPtr _midiInHandle = IntPtr.Zero;
        private MidiInProc _midiInProc;

        // コールバックデリゲート
        private delegate void MidiInProc(IntPtr hMidiIn, uint wMsg, IntPtr dwInstance, uint dwParam1, uint dwParam2);

        private OscSender oscSender;

        // MIDIのオープン状態
        public bool isOpen;
        // MIDI受信ログの表示行数
        private const int MAX_NUM_ITEMS = 10;
        // MIDIデバイス情報保管用
        Dictionary<string, uint> inDev = new Dictionary<string, uint>();
        // MIDIデバイスID
        private uint deviceId;
        // VRC接続状態
        private bool vrcConnect;
        // ピアノノート
        private static readonly string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        // 選択言語
        private int language;
        
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VRChatMidiTransferSystem()
        {
            InitializeComponent();

            this.MaximizeBox = false;
        }

        /// <summary>
        /// 画面ロード時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            SetupListView();
            SearchDevice();

            IP1.Text = "127";
            IP2.Text = "0";
            IP3.Text = "0";
            IP4.Text = "1";

            Port.Text = "9000";        

        }

        /// <summary>
        /// アプリ終了時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_midiInHandle != IntPtr.Zero)
            {
                midiInStop(_midiInHandle);
                midiInClose(_midiInHandle);
            }
            oscSender.Close();
        }

        /// <summary>
        /// リサーチボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReSearchButton_Click(object sender, EventArgs e)
        {
            SearchDevice();
            VRCConnection.Enabled = false;
        }

        /// <summary>
        /// デバイスコンボボックス変更時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            deviceId = inDev[DeviceComboBox.Text];
            VRCConnection.Enabled = true;
        }

        /// <summary>
        /// LogListViewの設定
        /// </summary>
        private void SetupListView()
        {
            // ListViewの設定
            LogView.View = View.Details;
            LogView.FullRowSelect = true;
            LogView.Columns.Add("Time", 80); // 時刻
            LogView.Columns.Add("Note", 50); // ノートタイプ
            LogView.Columns.Add("Data", 120); // データ

        }

        /// <summary>
        /// MIDIデバイスの取得
        /// </summary>
        public void SearchDevice()
        {
            DeviceComboBox.Items.Clear();
            DeviceComboBox.Text = "";
            inDev.Clear();

            // 接続されているMIDIデバイスの数取得
            uint numDevs = (uint)midiInGetNumDevs();
            if (numDevs > 0)
            {
                MidiInCaps inCaps = new MidiInCaps();
                for (uint dev = 0; dev < numDevs; ++dev)
                {
                    // 接続されているMIDIデバイスの情報取得
                    MMResult res = midiInGetDevCaps(dev, ref inCaps, (uint)Marshal.SizeOf(inCaps));
                    if (res == MMResult.NoError)
                    {
                        inDev.Add(inCaps.deviceName, dev);
                        DeviceComboBox.Items.Add(inCaps.deviceName);
                    }
                }
            }
        }

        /// <summary>
        /// MIDIデバイスオープン
        /// </summary>
        private void MidiInOpen()
        {
            _midiInProc = new MidiInProc(MidiCallback);

            MMResult result = MMResult.NoError;

            if (!isOpen)
            {
                // MIDIデバイスをオープン
                result = midiInOpen(ref _midiInHandle, (uint)deviceId, _midiInProc, deviceId, (uint)MidiCallbackFlags.Function | (uint)MidiCallbackFlags.MidiIOStatus);

                if (result != MMResult.NoError)
                {
                    if (LogView.Items.Count >= MAX_NUM_ITEMS)
                    {
                        LogView.Items.RemoveAt(0);
                    }
                    var item = new ListViewItem(DateTime.Now.ToString(Resource.Time, CultureInfo.InvariantCulture));
                    item.SubItems.Add("ERROR");
                    item.SubItems.Add(Resource.OpenError);
                    LogView.Items.Add(item);
                    return;
                }

                isOpen = true;

                // MIDIデバイスの受信開始
                result = midiInStart(_midiInHandle);

                if (result != MMResult.NoError)
                {
                    if (LogView.Items.Count >= MAX_NUM_ITEMS)
                    {
                        LogView.Items.RemoveAt(0);
                    }
                    var item = new ListViewItem(DateTime.Now.ToString(Resource.Time, CultureInfo.InvariantCulture));
                    item.SubItems.Add("ERROR");
                    item.SubItems.Add(Resource.reception_failed);
                    LogView.Items.Add(item);
                    midiInClose(_midiInHandle);
                    return;
                }
            }
        }

        /// <summary>
        /// MIDIデバイスクローズ
        /// </summary>
        private void MidiInClose()
        {
            MMResult result = MMResult.NoError;

            if (isOpen)
            {
                // MIDIデバイスの受信停止
                result = midiInStop(_midiInHandle);
                if (result != MMResult.NoError)
                {
                    if (LogView.Items.Count >= MAX_NUM_ITEMS)
                    {
                        LogView.Items.RemoveAt(0);
                    }
                    var item = new ListViewItem(DateTime.Now.ToString(Resource.Time, CultureInfo.InvariantCulture));
                    item.SubItems.Add("ERROR");
                    item.SubItems.Add(Resource.StopError);
                    LogView.Items.Add(item);
                    return;
                }

                // MIDIデバイスのクローズ
                result = midiInClose(_midiInHandle);

                if (result != MMResult.NoError)
                {
                    if (LogView.Items.Count >= MAX_NUM_ITEMS)
                    {
                        LogView.Items.RemoveAt(0);
                    }
                    var item = new ListViewItem(DateTime.Now.ToString(Resource.Time, CultureInfo.InvariantCulture));
                    item.SubItems.Add("ERROR");
                    item.SubItems.Add(Resource.disconnection_failed);
                    LogView.Items.Add(item);
                    midiInClose(_midiInHandle);
                    return;
                }

                isOpen = false;
            }
        }

        /// <summary>
        /// MIDIデバイスからのデータ受信を検知
        /// </summary>
        /// <param name="hMidiIn"></param>
        /// <param name="wMsg"></param>
        /// <param name="dwInstance"></param>
        /// <param name="dwParam1"></param>
        /// <param name="dwParam2"></param>
        private void MidiCallback(IntPtr hMidiIn, uint wMsg, IntPtr dwInstance, uint dwParam1, uint dwParam2)
        {
            // MIDIメッセージの処理
            if (wMsg == (uint)MidiMsgTypes.MM_MIM_DATA)
            {
                if (dwParam1 == 0xFE) { return; }

                // MIDIメッセージを解析
                uint status = (dwParam1 & 0xFF);
                uint data1 = ((dwParam1 >> 8) & 0xFF);
                uint data2 = ((dwParam1 >> 16) & 0xFF);
                string messageType = GetMessageType((byte)status);
                string data = FormatMidiMsgForDisplay(status, data1, data2);

                // ListViewに追加（UIスレッドで更新）
                Invoke((MethodInvoker)(() =>
                {
                    var item = new ListViewItem(DateTime.Now.ToString(Resource.Time, CultureInfo.InvariantCulture));
                    item.SubItems.Add(messageType);
                    item.SubItems.Add(data);
                    LogView.Items.Add(item);

                    if (LogView.Items.Count >= MAX_NUM_ITEMS)
                    {
                        LogView.Items.RemoveAt(0);
                    }
                }));

                // MIDI入力値によってOSCに送るデータを変更
                switch (status)
                {
                    case 144:
                        oscSender.Send(new OscMessage("/avatar/parameters/" + FormatMidiMsg(status, data1, data2), true));
                        break;
                    case 128:
                        oscSender.Send(new OscMessage("/avatar/parameters/" + FormatMidiMsg(status, data1, data2), false));
                        break;
                    case 176:
                        if (data2 == 0)
                        {
                            oscSender.Send(new OscMessage("/avatar/parameters/" + FormatMidiMsg(status, data1, data2), true));
                        }
                        else
                        {
                            oscSender.Send(new OscMessage("/avatar/parameters/" + FormatMidiMsg(status, data1, data2), false));
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// MIDIログ用メッセージ
        /// </summary>
        /// <param name="status">MIDI受信データ</param>
        /// <returns></returns>
        private string GetMessageType(byte status)
        {
            // MIDIステータスバイトからメッセージタイプを取得
            switch (status & 0xF0)
            {
                case 0x80: return "Off";
                case 0x90: return "On";
                case 0xA0: return "Polyphonic Key Pressure";
                case 0xB0: return "Pedal";
                case 0xC0: return "Program Change";
                case 0xD0: return "Channel Pressure";
                case 0xE0: return "Pitch Bend";
                default: return $"Unknown (0x{status:X2})";
            }
        }

        /// <summary>
        /// MIDIデバイスの受信データを変換
        /// </summary>
        /// <param name="status">受信タイプ</param>
        /// <param name="note">キー番号</param>
        /// <param name="velocity">キーの状態</param>
        /// <returns></returns>
        public string FormatMidiMsgForDisplay(uint status,uint note, uint velocity)
        {
            switch(language)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
                    break;
                case 2:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ko");
                    break;
            }
            if (note < 0 || note > 127)
            { return Resource.illegal_signal; }

            int noteNum = 0;
            int octave = 0;

            noteNum = (int)note % 12;
            octave = ((int)note - 12) / 12;

            if (status == 176)
            {
                return string.Format("{0}：{1}", Resource.Value, velocity);
            }
            else
            {
                return string.Format("{0}{1}　{2}：{3}", noteNames[noteNum], octave, Resource.strength, velocity);
            }
        }

        /// <summary>
        /// MIDIデバイスの受信データを変換
        /// </summary>
        /// <param name="status">受信タイプ</param>
        /// <param name="note">キー番号</param>
        /// <param name="velocity">キーの状態</param>
        /// <returns></returns>
        public string FormatMidiMsg(uint status, uint note, uint velocity)
        {

            if (note < 0 || note > 127)
            { return ""; }

            int noteNum = 0;
            int octave = 0;

            noteNum = (int)note % 12;
            octave = ((int)note - 12) / 12;

            string oscNote = noteNames[noteNum].Replace('#', 's');

            if (status == 176)
            {
                return "pedal";
            }
            else
            {
                return oscNote + octave;
            }
        }

        /// <summary>
        /// OSCオプション設定のチェック変更時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OSCOptCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (OSCOptCheck.Checked == true)
            {
                IP1.Enabled = true;
                IP2.Enabled = true;
                IP3.Enabled = true;
                IP4.Enabled = true;
                Port.Enabled = true;
            }
            else
            {
                IP1.Enabled = false;
                IP2.Enabled = false;
                IP3.Enabled = false;
                IP4.Enabled = false;
                Port.Enabled = false;
            }
        }

        /// <summary>
        /// 接続ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VRCConnection_Click(object sender, EventArgs e)
        {
            if (VRCConnection.Text == Resource.button)
            {
                try
                {
                    MidiInOpen();
                    if (isOpen == true)
                    {
                        DeviceComboBox.Enabled = false;
                        ReSearchButton.Enabled = false;
                        string ad = IP1.Text + "." + IP2.Text + "." + IP3.Text + "." + IP4.Text;
                        IPAddress address = IPAddress.Parse(ad);
                        int sendPort = int.Parse(Port.Text);
                        oscSender = new OscSender(address, 0, sendPort);
                        oscSender.Connect();

                        vrcConnect = true;
                        OSCOptCheck.Enabled = false;
                        txtdisplayCheck.Enabled = false;
                        textBox1.Enabled = false;
                        VRCConnection.Text = Resource.disconnection;

                        if (txtdisplayCheck.Checked)
                        {
                            oscSender.Send(new OscMessage("/chatbox/input", textBox1.Text));
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(Resource.ConnectionError + '\n' + ex.Message);
                }
            }
            else
            {
                try
                {
                    MidiInClose();
                    if (isOpen == false)
                    {
                        DeviceComboBox.Enabled = true;
                        ReSearchButton.Enabled = true;
                        LogView.Items.Clear();
                        oscSender.Close();
                        VRCConnection.Text = Resource.button;
                        vrcConnect = false;
                        OSCOptCheck.Enabled = true;
                        txtdisplayCheck.Enabled = true;
                        textBox1.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resource.disconnectionError + '\n' + ex.Message);
                }
            }
        }

        /// <summary>
        /// クレジットボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Credit_Click(object sender, EventArgs e)
        {
            string str = Resource.credittext;
            MessageBox.Show(str,Resource.credit,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        #region 言語設定ボタン

        private void Japanease_Click(object sender, EventArgs e)
        {
            Japanese.Checked = true;
            English.Checked = false;
            Korean.Checked = false;
            language = 0;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            LanguageSetting();
        }

        private void English_Click(object sender, EventArgs e)
        {
            Japanese.Checked = false;
            English.Checked = true;
            Korean.Checked = false;
            language = 1;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            LanguageSetting();
        }

        private void Korean_Click(object sender, EventArgs e)
        {
            Japanese.Checked = false;
            English.Checked = false;
            Korean.Checked = true;
            language = 2;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ko");
            LanguageSetting();
        }

        private void LanguageSetting()
        {
            groupBox1.Text = Resource.groupbox1;
            label2.Text = Resource.label2;
            label3.Text = Resource.label3;
            groupBox2.Text = Resource.groupbox2;
            OSCOptCheck.Text = Resource.checkbox1;
            label5.Text = Resource.label5;
            label6.Text = Resource.label6;
            txtdisplayCheck.Text = Resource.checkbox2;
            Japanese.Text = Resource.Japanese;
            English.Text = Resource.English;
            Korean.Text = Resource.Korean;
            Credit.Text = Resource.credit;            
            textBox1.Text = Resource.message;
            if(vrcConnect)
            {
                VRCConnection.Text = Resource.disconnection;
            }
            else
            {
                VRCConnection.Text = Resource.button;
            }
        }
        #endregion

        #region テキスト入力制限
        private void IP1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセル
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void IP2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセル
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void IP3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセル
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void IP4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセル
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセル
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}
