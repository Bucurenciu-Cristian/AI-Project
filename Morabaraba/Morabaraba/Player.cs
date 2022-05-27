using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    public class Player
    {
        public enum PlayerState
        {
            Placing,
            Moving,
            Taking,
            Flying
        }
        private bool myTurn;
        private bool myColor;//0 alb, 1 negru care are mai mare valoarea la un random ia alb
        private List<BoardCell> myHandCells;
        private List<BoardCell> myBoardCells;
        private PlayerState myState;
        private Mill[] myMills;
        private string myName = "";
        public Player(bool myTurn, bool myColor, string name)
        {
            this.myTurn = myTurn;
            this.myColor = myColor;
            this.myState = PlayerState.Placing;
            this.myName = name;
            this.myHandCells = new List<BoardCell>();
            this.myBoardCells = new List<BoardCell>();
            this.myMills = new Mill[4];
            InitializeMills();
            InitializeHandCells();
        }
        public void InitializeMills()
        {
            for(int i= 0; i < myMills.Length; i++)
            {
                myMills[i] = new Mill();
            }
        }
        public void InitializeHandCells()
        {
            for(int i=0;i<12;i++)//12 vaci are fiecare player
            {
                myHandCells.Add(new BoardCell());
            }
        }
        public System.Drawing.Bitmap DrawMyColor()
        {
            if (myColor == false)
            {
                return Properties.Resources.blackPiece;
            }
            else
            {
                return Properties.Resources.whitePiece;
            }
        }
        public void PrintPlayerMill(Player activePlayer)
        {
            for (int i = 0; i < activePlayer.GetMyMills().Count(); i++)
            {
                if (GamePlay.CheckMillIsNew(activePlayer.GetMyMills()[i]))
                {
                    Debug.WriteLine("Moara" + i);
                    for (int j = 0; j < activePlayer.GetMyMills()[i].GetMillCells().Count(); j++)
                    {
                        Debug.Write(activePlayer.GetMyMills()[i].GetMillCells()[j].GetId() + " ");
                    }
                    Debug.WriteLine("");
                }
            }
        }
        public bool GetMyTurn()
        {
            return myTurn;
        }
        public void SetMyTurn(bool myTurn)
        {
            this.myTurn = myTurn;
        }
        public bool GetMyColor()
        {
            return myColor;
        }
        public void SetMyColor(bool myColor)
        {
            this.myColor = myColor;
        }
        public List<BoardCell> GetMyHandCells()
        {
            return myHandCells;
        }
        public void SetMyHandCells(List<BoardCell> myHandCells)
        {
            this.myHandCells = myHandCells;
        }
        public List<BoardCell> GetMyBoardCells()
        {
            return this.myBoardCells;
        }
        public void SetMyBoardCells(List<BoardCell> myBoardCells)
        {
            this.myBoardCells = myBoardCells;
        }
        public Mill[] GetMyMills()
        {
            return this.myMills;
        }
        public void SetMyMills(Mill[] myMills)
        {
            this.myMills = myMills;
        }
        public PlayerState GetMyState()
        {
            return myState;
        }
        public void SetMyState(PlayerState myState)
        {
            this.myState = myState;
        }
        public string GetMyName()
        {
            return this.myName;
        }
        public void SetMyName(string myName)
        {
            this.myName = myName;
        }
    }
}
