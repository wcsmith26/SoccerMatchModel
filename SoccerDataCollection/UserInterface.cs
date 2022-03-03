using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace SoccerDataCollection
{
    public partial class UserInterface : Form
    {
        /// <summary>
        /// Indicates what player a pass came from.
        /// </summary>
        private int _passOrigin = -1;

        /// <summary>
        /// Indicates what player a pass arrived at.
        /// </summary>
        private int _passDest = -1;

        /// <summary>
        /// Indicates which player took the last shot.
        /// </summary>
        private int _shotFrom = -1;

        /// <summary>
        /// Stack that contains all of the passes made in the game.
        /// </summary>
        private Stack<(int, int)> _stack = new Stack<(int, int)>();

        /// <summary>
        /// A 2D array that contains all of the amount of passes from 1 player to another.
        /// </summary>
        private int[,] _passData = new int[11, 11];

        /// <summary>
        /// An array that contains the number of shots each player has taken.
        /// </summary>
        private int[] _shotData = new int[11];

        /// <summary>
        /// An array that contains the number of shots on target each player has.
        /// </summary>
        private int[] _onTarget = new int[11];

        /// <summary>
        /// An array that contains the number of goals each player has scored.
        /// </summary>
        private int[] _goalData = new int[11];

        /// <summary>
        /// An array that contains all the name labels.
        /// </summary>
        private Control[] _nameLabels = new Control[11];

        /// <summary>
        /// An array that contains all the position labels.
        /// </summary>
        private Control[] _posLabels = new Control[11];

        public UserInterface()
        {
            InitializeComponent();
            InitializeLabelArrays();
        }

        public void InitializeLabelArrays()
        {
            _nameLabels[0] = uxName1Label;
            _nameLabels[1] = uxName2Label;
            _nameLabels[2] = uxName3Label;
            _nameLabels[3] = uxName4Label;
            _nameLabels[4] = uxName5Label;
            _nameLabels[5] = uxName6Label;
            _nameLabels[6] = uxName7Label;
            _nameLabels[7] = uxName8Label;
            _nameLabels[8] = uxName9Label;
            _nameLabels[9] = uxName10Label;
            _nameLabels[10] = uxName11Label;

            _posLabels[0] = uxPos1Label;
            _posLabels[1] = uxPos2Label;
            _posLabels[2] = uxPos3Label;
            _posLabels[3] = uxPos4Label;
            _posLabels[4] = uxPos5Label;
            _posLabels[5] = uxPos6Label;
            _posLabels[6] = uxPos7Label;
            _posLabels[7] = uxPos8Label;
            _posLabels[8] = uxPos9Label;
            _posLabels[9] = uxPos10Label;
            _posLabels[10] = uxPos11Label;
        }

        public void PassMadeDisableAll()
        {
            uxPassMade1Btn.Enabled = false;
            uxPassMade2Btn.Enabled = false;
            uxPassMade3Btn.Enabled = false;
            uxPassMade4Btn.Enabled = false;
            uxPassMade5Btn.Enabled = false;
            uxPassMade6Btn.Enabled = false;
            uxPassMade7Btn.Enabled = false;
            uxPassMade8Btn.Enabled = false;
            uxPassMade9Btn.Enabled = false;
            uxPassMade10Btn.Enabled = false;
            uxPassMade11Btn.Enabled = false;
        }

        public void PassMadeEnableAll()
        {
            uxPassMade1Btn.Enabled = true;
            uxPassMade2Btn.Enabled = true;
            uxPassMade3Btn.Enabled = true;
            uxPassMade4Btn.Enabled = true;
            uxPassMade5Btn.Enabled = true;
            uxPassMade6Btn.Enabled = true;
            uxPassMade7Btn.Enabled = true;
            uxPassMade8Btn.Enabled = true;
            uxPassMade9Btn.Enabled = true;
            uxPassMade10Btn.Enabled = true;
            uxPassMade11Btn.Enabled = true;
        }

        public void PassReceivedDiableAll()
        {
            uxPassReceived1Btn.Enabled = false;
            uxPassReceived2Btn.Enabled = false;
            uxPassReceived3Btn.Enabled = false;
            uxPassReceived4Btn.Enabled = false;
            uxPassReceived5Btn.Enabled = false;
            uxPassReceived6Btn.Enabled = false;
            uxPassReceived7Btn.Enabled = false;
            uxPassReceived8Btn.Enabled = false;
            uxPassReceived9Btn.Enabled = false;
            uxPassReceived10Btn.Enabled = false;
            uxPassReceived11Btn.Enabled = false;
            uxTurnoverBtn.Enabled = false;
        }

        public void PassReceivedEnableAll()
        {
            uxPassReceived1Btn.Enabled = true;
            uxPassReceived2Btn.Enabled = true;
            uxPassReceived3Btn.Enabled = true;
            uxPassReceived4Btn.Enabled = true;
            uxPassReceived5Btn.Enabled = true;
            uxPassReceived6Btn.Enabled = true;
            uxPassReceived7Btn.Enabled = true;
            uxPassReceived8Btn.Enabled = true;
            uxPassReceived9Btn.Enabled = true;
            uxPassReceived10Btn.Enabled = true;
            uxPassReceived11Btn.Enabled = true;
            uxTurnoverBtn.Enabled = true;
        }

        public void ShotDiableAll()
        {
            uxShot1Btn.Enabled  = false;
            uxShot2Btn.Enabled  = false;
            uxShot3Btn.Enabled  = false;
            uxShot4Btn.Enabled  = false;
            uxShot5Btn.Enabled  = false;
            uxShot6Btn.Enabled  = false;
            uxShot7Btn.Enabled  = false;
            uxShot8Btn.Enabled  = false;
            uxShot9Btn.Enabled  = false;
            uxShot10Btn.Enabled = false;
            uxShot11Btn.Enabled = false;
            uxGoalBtn.Enabled = true;
            uxOnTargetBtn.Enabled = true;
            uxOffTargetBtn.Enabled = true;
        }

        public void ShotEnableAll()
        {
            uxShot1Btn.Enabled  = true;
            uxShot2Btn.Enabled  = true;
            uxShot3Btn.Enabled  = true;
            uxShot4Btn.Enabled  = true;
            uxShot5Btn.Enabled  = true;
            uxShot6Btn.Enabled  = true;
            uxShot7Btn.Enabled  = true;
            uxShot8Btn.Enabled  = true;
            uxShot9Btn.Enabled  = true;
            uxShot10Btn.Enabled = true;
            uxShot11Btn.Enabled = true;
            uxGoalBtn.Enabled = false;
            uxOnTargetBtn.Enabled = false;
            uxOffTargetBtn.Enabled = false;
        }

        public void PassMadeHelper()
        {
            PassReceivedEnableAll();
            PassMadeDisableAll();
            ShotDiableAll();
            uxOnTargetBtn.Enabled = false;
            uxGoalBtn.Enabled = false;
            uxOffTargetBtn.Enabled = false;
        }

        public void PassReceivedHelper()
        {
            PassMadeEnableAll();
            PassReceivedDiableAll();
            ShotEnableAll();
        }

        public void StackPass(int origin, int dest)
        {
            (int, int) passTuple = (origin, dest);
            _stack.Push(passTuple);
            if (_stack.Count > 0)
            {
                uxUndoBtn.Enabled = true;
            }
        }

        /// <summary>
        /// Creates a tuple cotaining information about who shot the ball and what type of shot it was.
        /// </summary>
        /// <param name="origin">The index of the player who shot the ball.</param>
        /// <param name="type">Indicates if the shot was a goal (-1), on target (-2), or off target (-3).</param>
        public void StackShot (int origin, int type)
        {
            (int, int) shotTuple = (origin, type);
            _stack.Push(shotTuple);
            if(_stack.Count > 0)
            {
                uxUndoBtn.Enabled = true;
            }
        }

        private void uxUndoBtn_Click(object sender, EventArgs e)
        {
            (int, int) removed = _stack.Pop();
            if (removed.Item2 < 0)
            {
                if (removed.Item2 == -1)
                {
                    _shotData[removed.Item1]--;
                    _onTarget[removed.Item1]--;
                    _goalData[removed.Item1]--;
                }
                else if (removed.Item2 == -2)
                {
                    _onTarget[removed.Item1]--;
                    _shotData[removed.Item1]--;
                }
                else
                {
                    _shotData[removed.Item1]--;
                }
            }
            else
            {
                _passData[removed.Item1, removed.Item2]--;
            }
            if (_stack.Count == 0)
            {
                uxUndoBtn.Enabled = false;
            }
        }

        private void uxPassMade1Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 0;
            PassMadeHelper();
            uxPassReceived1Btn.Enabled = false;
        }

        private void uxPassMade2Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 1;
            PassMadeHelper();
            uxPassReceived2Btn.Enabled = false;
        }

        private void uxPassMade3Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 2;
            PassMadeHelper();
            uxPassReceived3Btn.Enabled = false;
        }

        private void uxPassMade4Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 3;
            PassMadeHelper();
            uxPassReceived4Btn.Enabled = false;
        }

        private void uxPassMade5Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 4;
            PassMadeHelper();
            uxPassReceived5Btn.Enabled = false;
        }

        private void uxPassMade6Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 5;
            PassMadeHelper();
            uxPassReceived6Btn.Enabled = false;
        }

        private void uxPassMade7Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 6;
            PassMadeHelper();
            uxPassReceived7Btn.Enabled = false;
        }

        private void uxPassMade8Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 7;
            PassMadeHelper();
            uxPassReceived8Btn.Enabled = false;
        }

        private void uxPassMade9Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 8;
            PassMadeHelper();
            uxPassReceived9Btn.Enabled = false;
        }

        private void uxPassMade10Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 9;
            PassMadeHelper();
            uxPassReceived10Btn.Enabled = false;
        }

        private void uxPassMade11Btn_Click(object sender, EventArgs e)
        {
            _passOrigin = 10;
            PassMadeHelper();
            uxPassReceived11Btn.Enabled = false;
        }

        private void uxPassReceived1Btn_Click(object sender, EventArgs e)
        {
            _passDest = 0;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived2Btn_Click(object sender, EventArgs e)
        {
            _passDest = 1;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived3Btn_Click(object sender, EventArgs e)
        {
            _passDest = 2;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived4Btn_Click(object sender, EventArgs e)
        {
            _passDest = 3;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived5Btn_Click(object sender, EventArgs e)
        {
            _passDest = 4;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived6Btn_Click(object sender, EventArgs e)
        {
            _passDest = 5;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived7Btn_Click(object sender, EventArgs e)
        {
            _passDest = 6;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived8Btn_Click(object sender, EventArgs e)
        {
            _passDest = 7;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived9Btn_Click(object sender, EventArgs e)
        {
            _passDest = 8;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived10Btn_Click(object sender, EventArgs e)
        {
            _passDest = 9;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxPassReceived11Btn_Click(object sender, EventArgs e)
        {
            _passDest = 10;
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxTurnoverBtn_Click(object sender, EventArgs e)
        {
            _passDest = _passOrigin; //The number along the diagonal indicates the number of incomplete passes.
            _passData[_passOrigin, _passDest]++;
            PassReceivedHelper();
            StackPass(_passOrigin, _passDest);
        }

        private void uxShot1Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 0;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot2Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 1;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot3Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 2;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot4Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 3;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot5Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 4;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot6Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 5;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot7Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 6;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot8Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 7;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot9Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 8;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot10Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 9;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxShot11Btn_Click(object sender, EventArgs e)
        {
            _shotFrom = 10;
            _shotData[_shotFrom]++;
            ShotDiableAll();
        }

        private void uxOffTargetBtn_Click(object sender, EventArgs e)
        {
            if (_shotFrom == -1)
            {
                return;
            }
            StackShot(_shotFrom, -3);
            _shotFrom = -1;
            ShotEnableAll();
        }

        private void uxOnTargetBtn_Click(object sender, EventArgs e)
        {
            if (_shotFrom == -1)
            {
                return;
            }
            _onTarget[_shotFrom]++;
            StackShot(_shotFrom, -2);
            _shotFrom = -1;
            ShotEnableAll();
        }

        private void uxGoalBtn_Click(object sender, EventArgs e)
        {
            if (_shotFrom == -1)
            {
                return;
            }
            _goalData[_shotFrom]++;
            _onTarget[_shotFrom]++;
            StackShot(_shotFrom, -1);
            _shotFrom = -1;
            ShotEnableAll();
        }

        /// <summary>
        /// Imports information from the team sheet. Changes the labels of the player names and positions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openTeamSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uxOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = uxOpenFileDialog.FileName;

                using (StreamReader sr = new StreamReader(fileName))
                {
                    int count = 0;
                    while (!sr.EndOfStream)
                    {
                        string temp = sr.ReadLine();
                        string[] split = temp.Split(' ');
                        string number = split[0];
                        string name;
                        string pos;
                        if (split.Length == 3)
                        {
                            name = split[1];
                            pos = split[2];
                        }
                        else
                        {
                            name = split[1] + split[2];
                            pos = split[3];
                        }

                        _nameLabels[count].Text = name;
                        _posLabels[count].Text = pos;
                        count++;
                    }
                }
            }
        }

        /// <summary>
        /// Converts the _passData array to a CSV file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savePassingDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uxSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = uxSaveFileDialog.FileName;

                using (StreamWriter sw = new StreamWriter(filename))
                {
                    for (int i = 0; i < 11; i++)
                    {
                        sw.Write(_nameLabels[i].Text + ",");
                        if (i == 10)
                        {
                            sw.WriteLine("");
                        }
                    }
                    for (int i = 0; i < 11; i++)
                    {
                        sw.Write(_nameLabels[i].Text + ",");
                        sw.Write(_posLabels[i].Text + ",");
                        for (int j = 0; j < 11; j++)
                        {
                            sw.Write(_passData[i, j].ToString() + ",");
                        }
                        sw.Write(_goalData[i].ToString() + "," + _onTarget[i].ToString() + "," + _shotData[i].ToString() + ",");
                        sw.WriteLine("");
                    }
                }
            }
        }
    }
}
