using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    public class Player
    {
        //culoare
        //nr piese
        //state
        //randul meu, dupa un timp sa schimb automat randul si sa mut orice?
        //maxim 4 mori
        //avem cazul player vs player sau player vs ia
        public enum PlayerState
        {
            Placing,
            Moving,
            Taking,//pentru moara sa ia piesa adversarului
            Flying
        }
        private bool myTurn;
        private bool myColor;//0 alb, 1 negru care are mai mare valoarea la un random ia alb
        private List<BoardCell> myHandCells;
        private List<BoardCell> myBoardCells;// variabila de tinut minte o celula pe care am apasat-o pentru miscare
        // cell apasat ( selectata ) = new cell, daca e apasata aceeasi celula de 2 ori sa o deselecteze 
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
