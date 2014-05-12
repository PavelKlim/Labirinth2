using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Labirinth2
{
    class Program
    {
      // 1.Read input data from file lab.txt to field of cell's array cellStatus
      // 2.Find the start of labirinth. Find a from labirinth and set cellStatus "Begin"
      // 3.Check cases from 4 corners to find free space ("0")
      // 4.wave=1, Set the value of wave to free cells round (4 sides) cell[x][y]=wave
      // 5.repeat item 4, while finish not found
      // 6.make the shortest way from finish to start. From (wave-1) to wave=1

        public enum cellStatus {Empty, Wall, Start, End};

        public class cell
        {
            public int wave;
            public char symbol;
            public cellStatus status;


            public cell(int wave,cellStatus status,char symbol)
            {
                this.wave = wave;
                this.status = status;
                this.symbol = symbol;
            }
        }
            public class labLegend //info about symbols, start and end in labirinth
            {
                public char start;
                public char end;
                public char empty;
                public char wall;
                public coord startPos;
                public coord endPos;

                public labLegend(char start, char end, char empty, char wall, coord startPos, coord endPos)
                {
                    this.start = start;
                    this.end = end;
                    this.empty = empty;
                    this.wall = wall;

                    this.startPos = startPos;
                    this.endPos = endPos;
                }

                
            }

            public class coord //coordinates
            {
                public int x;
                public int y;

                public coord(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }
            }





            static bool checkStartandEnd(string[] lab, ref labLegend lab1) //use to check and find start and finish
            {
                lab1.startPos = null;
                lab1.endPos = null;
                for (int i = 0; i < lab.Length; i++)
                {
                    for (int j = 0; j < lab[i].Length; j++)
                        if (lab[i][j] == lab1.start)
                        {
                            lab1.startPos = new coord(j,i);
                        }
                        else
                            if (lab[i][j] == lab1.end)
                            {
                                lab1.endPos = new coord(j, i);
                            }
                }
                if (lab1.startPos != null && lab1.endPos != null)
                    return true;
                else
                    return false;
            }

            static cell[,] labCreator(string[] lab,labLegend labLeg,out coord labSize) //create lab structure from string array and legend info
            {
                  
                  
                labSize = null;
                if (lab != null)
                {
                    labSize = new coord(lab[0].Length, lab.Length);
                }
                  
                cell[,] labirinth = new cell[labSize.y, labSize.x];

                                  //for (int h = 0; h < labSize.y;h++ )
                                    //  labirinth[i] = labRow;

                  

                if (lab != null && labLeg != null)
                {
                    for (int i = 0; i < labSize.y; i++)
                    {
                        for (int j = 0; j < labSize.x; j++)
                        {
                            if(lab[i][j] == labLeg.empty)
                            labirinth[i,j] = new cell(0,cellStatus.Empty,labLeg.empty);
                        else
                            labirinth[i,j] = new cell(-1,cellStatus.Wall,labLeg.wall);
                        }
                    }
                    labirinth[labLeg.startPos.y,labLeg.startPos.x] = new cell(0,cellStatus.Start, labLeg.start);
                    labirinth[labLeg.endPos.y,labLeg.endPos.x] = new cell(-2,cellStatus.End, labLeg.end);
                      
                }
                return labirinth;
            }

            static bool cellCheck(cell a) //checking free cells
            {
                if (a.status == cellStatus.Empty)
                    return true;
                return false;
            }

            static bool cellSetWave(ref cell a, ref int wave) //seting the current value of wave in cell
            {
                if ((a != null && a.status == cellStatus.Empty && a.wave == 0) )
                {
                    a.wave = wave;
                    return true;
                }
                return false;
            }

        
            static void waveGen(coord curCell, ref cell[,] lab, int wave) //didn't finish
            {
            
                wave++;
                if (curCell.x + 1 < lab.GetLength(1))
                if (cellCheck(lab[curCell.y, curCell.x + 1]))
                    if (cellSetWave(ref lab[curCell.y, curCell.x + 1], ref wave))
                    {
                        curCell.x += 1;
                        waveGen(curCell,ref lab,wave);
                        curCell.x -= 1;
                    }

                if (curCell.x - 1 >=0)
                    if (cellCheck(lab[curCell.y, curCell.x - 1]))
                        if (cellSetWave(ref lab[curCell.y, curCell.x - 1], ref wave))
                        {
                            curCell.x -= 1;
                            waveGen(curCell, ref lab, wave);
                            curCell.x += 1;
                        }

                if (curCell.y + 1 < lab.GetLength(0) )
                    if (cellCheck(lab[curCell.y + 1, curCell.x]))
                        if (cellSetWave(ref lab[curCell.y + 1, curCell.x], ref wave))
                        {
                            curCell.y += 1;
                            waveGen(curCell, ref lab, wave);
                            curCell.y -= 1;
                        }

                if (curCell.y - 1 >= 0)
                    if (cellCheck(lab[curCell.y - 1, curCell.x]))
                        if (cellSetWave(ref lab[curCell.y - 1, curCell.x], ref wave))
                        {
                            curCell.y -= 1;
                            waveGen(curCell, ref lab, wave);
                            curCell.y += 1;
                        }
             
 
            }
        
            static public coord minValueFind(ref cell[,] lab, coord curCell)
            {
                coord res = null;
                if (lab[curCell.y - 1, curCell.x].wave == lab[curCell.y, curCell.x].wave - 1)
                {
                    curCell.y -= 1;
                    res = new coord(curCell.x, curCell.y);
                    curCell.y += 1;
                    return res;
                }

                if (lab[curCell.y + 1, curCell.x].wave == lab[curCell.y, curCell.x].wave - 1)
                {
                    curCell.y += 1;
                    res = new coord(curCell.x, curCell.y);
                    curCell.y -= 1;
                    return res;
                }

                if (lab[curCell.y, curCell.x - 1].wave == lab[curCell.y, curCell.x].wave - 1)
                {
                    curCell.x -= 1;
                    res = new coord(curCell.x, curCell.y);
                    curCell.x += 1;
                    return res;
                }

                if (lab[curCell.y, curCell.x + 1].wave == lab[curCell.y, curCell.x].wave - 1)
                {
                    curCell.x += 1;
                    res = new coord(curCell.x, curCell.y);
                    curCell.x -= 1;
                    return res;
                }
                return res;

            }

            static public coord[] wayFind(ref cell[,] lab, coord end)
            {
                coord[] res=new coord[1];
                res[0] = new coord(end.x, end.y);
                int i = 0;
                while (lab[end.y, end.x].wave != 0)
                {
                    end=minValueFind(ref lab, end);
                    if (end != null)
                    {
                        res[i] = new coord(end.x, end.y);
                        Array.Resize(ref res, res.Length + 1);
                        i++;
                    }
                    else
                        break;
                }

                return res;
            }
        
        
            static void Main(string[] args)
            {
                string[] lines= System.IO.File.ReadAllLines("lab.txt");

                for (int i = 0; i < lines.Length; i++)
                    Console.WriteLine(lines[i]);

                Console.WriteLine("From cells:"+"\n");

                labLegend labLeg1 = new labLegend('a', 'b', '0', '1', null, null);
                cell[,] lab1 = null;
                coord labSize = null;

                if (checkStartandEnd(lines, ref labLeg1))
                {
                    lab1=labCreator(lines,labLeg1,out labSize);
                }
                for (int i = 0; i < labSize.y; i++)
                {
                    for (int j = 0; j < labSize.x; j++)
                    {
                        Console.Write(lab1[i,j].symbol);
                    }
                    Console.Write("\n");
                }

                waveGen(labLeg1.startPos,ref lab1,0);
                Console.WriteLine("Wave: "+"\n");

                string space;
                int count;
                int wave = 0;
                wave=lab1[0,0].wave;

                for (int i = 0; i < labSize.y; i++)
                {
                    for (int j = 0; j < labSize.x; j++)
                    {
                        if (lab1[i, j].wave >= 0)
                        {
                            space = null;
                            count = 3 - lab1[i, j].wave.ToString().Length;
                            for (int b = 0; b < count; b++)
                                space = space + " ";

                            Console.Write(lab1[i, j].wave + space);
                            if (lab1[i, j].wave > wave)
                                wave = lab1[i, j].wave;
                        }
                        else
                            Console.Write("00 ");
                    }
                    Console.Write("\n");
                }
                lab1[labLeg1.endPos.y, labLeg1.endPos.x].wave = wave + 1;
                Console.WriteLine(lab1[labLeg1.endPos.y, labLeg1.endPos.x].wave);


                Console.WriteLine("Way home:"+"\n");
                coord[] wayHome=wayFind(ref lab1, labLeg1.endPos);

                for (int g = 0; g < wayHome.Length-1; g++)
                {
                    Console.WriteLine("x=" + wayHome[g].x + " y=" + wayHome[g].y);
                }
                                   
                    Console.ReadKey();
            }
    }
}
