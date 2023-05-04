// 0719581 - Dharamraj Patel
// 0680481 – Amber Ahmed


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3020Assignment1
{
   

    public enum Colour { EMPTY, RED, YELLOW, GREEN, BLUE,GREY}

    public class SubwayMap
    {

        private class Node
        {
            public Station Connection; // Adjacent station (connection)
            public Colour Line;  // Colour of its subway line
            public Node Next;     // Link to the next adjacent station (Node)



            //
            public Node()
            {
                Next = null; //
                Line = Colour.EMPTY;
                Connection = null;
            }


            public Node(Station connection, Colour c, Node next)
            {
                Connection = connection;
                Line = c;
                Next = next;
            }

        }
        //---------------------------------------------------------------------
        private class Station
        {
            public string Name;      // Name of the subway station
            public bool visited;     // Used for the breadth-first search
            public Node E;           // Header node for a linked list of adjacent stations

            public Station Parent;

            public int Count;      // Number of Adjecent stations

            public Station(string name)
            {
                Name = name;
                visited = false;
                Count = 0;
                E = new Node();
                Parent = null;

            }
        }

        //---------------------------------------------------------------------

        // Class data members

        private Dictionary<string, Station> S;          //Dictionary of stations

        public SubwayMap()
        {
            S = new Dictionary<string, Station>();

        }

        //---------------------------------------------------------------------

        public void InsertStation(string name)
        {
            if (!(S.ContainsKey(name)))
            {
                S.Add(name, new Station(name));
            }
        }

        //---------------------------------------------------------------------
        public bool RemoveStation(string name)
        {
            if (S.ContainsKey(name))
            {
                Node curr = S[name].E;

                // if the staion has more that one one connection
                // delete the connection from the adjecent stations and then delete the Key
                while (S[name].Count > 0)
                {
                    curr = curr.Next;
                    StationRemove(curr.Connection.Name, name);
                    S[name].Count -= 1;
                    
                }
                S.Remove(name);
                Console.WriteLine("Station '" + name + "' removed from the map");
                return true;
            }
            else
            {
                Console.WriteLine("Station not found");
                return false;
            }

        }

        public void StationRemove(string adj, string S_name)
        {
            bool found = false;
            Node newCurr = S[adj].E;
            while (newCurr.Next != null && !found)
            {

                if (newCurr.Next.Connection.Name.Equals(S_name))
                {
                    newCurr.Next = newCurr.Next.Next;
                    S[adj].Count--;
                    found = true;

                }
                else
                {
                    newCurr = newCurr.Next;
                }
            }

        }


        //---------------------------------------------------------------------

        public bool InsertConnection(string name1, string name2, Colour c)
        {
            // check if both stations exist
            if (S.ContainsKey(name1) && S.ContainsKey(name2))
            {
                Node curr = S[name1].E;
                Node front = S[name2].E;

                // if there are no adjecent connections to that station yet.
                if (S[name1].Count == 0)
                {
                    curr.Next = new Node(S[name2], c, curr.Next);
                    front.Next = new Node(S[name1], c, front.Next);
                    S[name1].Count += 1;
                    S[name2].Count += 1;
                    //found = true;
                    return true;
                }

                // if the station already has a few connections...
                // We go through the linked list to see if there is already a connection.
                int stationCount = 0;
                while (curr.Next != null || stationCount < S[name1].Count)
                {
                    // if the connection already exists...
                    if (curr.Next.Connection.Equals(S[name2]))
                    {
                        // if the connections exists on the same line
                        if (curr.Next.Line.Equals(c))
                        {
                            Console.WriteLine("Connection already exists");
                            return false;
                        }
                        // if there is a connection but not on the same line
                        // Add a connection
                        else
                        {
                            curr.Next = new Node(S[name2], c, curr.Next);
                            front.Next = new Node(S[name1], c, front.Next);
                            S[name1].Count += 1;
                            S[name2].Count += 1;
                            return true;
                        }
                    }
                    else
                    {
                        stationCount++;
                        curr = curr.Next;
                    }
                }


                // if there is no connection between those 2 stations
                curr.Next = new Node(S[name2], c, curr.Next);
                front.Next = new Node(S[name1], c, front.Next);
                S[name1].Count += 1;
                S[name2].Count += 1;
                //found = true;
                return true;
            }

            return false; // 
        }

        public bool stationFound(string name)
        {
            foreach (var item in S)
            {
                if (item.Key == name)
                {
                    Console.WriteLine("Duplicate station");
                    return true;
                }
            }
            return false;
        }

        //---------------------------------------------------------------------


        public bool RemoveConnection(string name1, string name2, Colour c)
        {
            if (S.ContainsKey(name1) && S.ContainsKey(name2))
            {
                // if Connection A-B exists
                // remove connection from station A to Station B
                if (ConnectionRemove(name1, name2, c) == true)
                {
                    // then remove the other connection
                    // remove connection from station B to Station A
                    ConnectionRemove(name2, name1, c);
                }
                return true;   // return true when the connection is removed
            }

            return false;  // connection not removed
        }


        // visit all nodes of station st1 , to remove the node(station) that contaions st2
        public bool ConnectionRemove(string st1, string st2, Colour C)
        {

            Node curr = S[st1].E;
            bool found = false;

            // go through the linked list 
            while(curr.Next != null && found == false)
            {
                // if the station name equals st.name && the colour is the same
                if(curr.Next.Connection.Name.Equals(st2) || curr.Next.Equals(C))
                {
                    curr.Next = curr.Next.Next; // remove that node
                    found = true;               // break the loop
                    
                }
                else
                {
                    curr = curr.Next;   // if node not found, go to the next node.
                } 
            }
            // if !found > meaning the link(connection) was not removed/found
            if (!found)
            {
                Console.WriteLine("No connection found");
                return false;
            }
            else
                return true; 
                
            
        }

        //---------------------------------------------------------------------


        public void SRoute(string name1, string name2)
        {
            
            if (S.ContainsKey(name1) && S.ContainsKey(name2))
            {
                Queue<Station> q = new Queue<Station>(); // QUEUE TO STORE THE STATIONS
                

                q.Enqueue(S[name1]); // DEPARTURE STATION

                S[name1].visited = true; //DEPARTURE STATION , VISITED TRUE

                bool sr = false;  // bool valu

                while (q.Count != 0)
                {
                    Station s = q.Dequeue();  // pop the station

                    if (s.Name == name2)   // 
                    {
                        Console.WriteLine(" Shortest route Found");
                        sr = true;
                        break;
                    }

                    Node curr = s.E; // SET THE NODE TO THE CURRENT STATION 

                    // going through the list of the station (Node E)
                    while (curr.Next != null)
                    {
                        if (!curr.Next.Connection.visited ) // if next station is not visited yet >  FALSE
                        {
                            q.Enqueue(curr.Next.Connection);       // push the next node to the queue
                            curr.Next.Connection.visited = true;   // visited = true
                            curr.Next.Connection.Parent = s;       // parent of the next node = current node
                            
                        }
                        curr = curr.Next;
                    }
                }
                // if shortest route is Found
                // Traversing from the destination station to the departing station 
                if (sr == true)  
                {
                    Station destination = S[name2];  // 
                    

                    // printing the stations in reverse ordder 
                    while (destination.Parent != null)
                    {
                        Console.WriteLine( destination.Name); // the station we are currenly pointing to

                        destination = destination.Parent;  // next parent station
                        
                    }
                    Console.WriteLine(name1 + "  > Departing station "); // this is the departing station
                }
                else
                {
                    Console.WriteLine("Route not Found");
                }


                // changing back the values of all station to FALSE > NOT visited
                foreach (Station nVis in S.Values)
                {
                    nVis.visited = false;
                    nVis.Parent = null;
                }

            }
            else
            {
                Console.WriteLine("No route since one of the station is not found");
            }
        }
   
    }



    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SubwayMap sm = new SubwayMap();

            sm.InsertStation("MANCHESTER");
            sm.InsertStation("CLIFTON");
            sm.InsertStation("SHEFFIELD");
            sm.InsertStation("LONDON");
            sm.InsertStation("OSTERLY");
            sm.InsertStation("HEATHROW");
            sm.InsertStation("READING");
            sm.InsertStation("SOUTHAMPTON");
            sm.InsertStation("PORTSMOUTH");
            sm.InsertStation("BRIGHTON");
            sm.InsertStation("EDINBURGH");
            sm.InsertStation("LEEDS");
            sm.InsertStation("OXFORD");
            sm.InsertStation("NEWBURY");
            sm.InsertStation("WINCHESTER");
            sm.InsertStation("MALBOROUGH");
            sm.InsertStation("WARMINISTER");
            sm.InsertStation("BOURMOUTH");
            sm.InsertStation("HULL");
            sm.InsertStation("COLCHESTER");
            sm.InsertStation("BATH");
            sm.InsertStation("BRISTOL");
            sm.InsertStation("CARDIFF");
            sm.InsertStation("PETERBOROUGH");
            sm.InsertStation("IPSWICH");
            sm.InsertStation("SUDBURY");
            sm.InsertStation("CLEMSFORD");
            sm.InsertStation("SWINDON");
            sm.InsertStation("LIVERPOOL");
            sm.InsertStation("BIRMINGHAM");
            sm.InsertStation("CAMBRIDGE");
            sm.InsertStation("NORWICH");

            Console.WriteLine("===========");

            // RED LINE ==> CONNECTIONS
            sm.InsertConnection("MANCHESTER","CLIFTON",Colour.RED);
            sm.InsertConnection("CLIFTON","SHEFFIELD", Colour.RED);
            sm.InsertConnection("SHEFFIELD","LONDON", Colour.RED);
            sm.InsertConnection("LONDON","OSTERLY", Colour.RED);
            sm.InsertConnection("OSTERLY","HEATHROW",Colour.RED);
            sm.InsertConnection("HEATHROW", "READING", Colour.RED);
            sm.InsertConnection("READING", "SOUTHAMPTON", Colour.RED);
            sm.InsertConnection("SOUTHAMPTON", "PORTSMOUTH", Colour.RED);
            sm.InsertConnection("PORTSMOUTH", "BRIGHTON", Colour.RED);

            // GREEN LINE  ==> CONNECTIONS
            sm.InsertConnection("LIVERPOOL","BIRMINGHAM",Colour.GREEN);
            sm.InsertConnection("BIRMINGHAM","LONDON", Colour.GREEN);
            sm.InsertConnection("LONDON","CAMBRIDGE", Colour.GREEN);
            sm.InsertConnection("CAMBRIDGE","COLCHESTER", Colour.GREEN);
            sm.InsertConnection("COLCHESTER","IPSWICH", Colour.GREEN);
            sm.InsertConnection("IPSWICH","NORWICH", Colour.GREEN);

            // BLUE LINE ==> CONNECTIONS
            sm.InsertConnection("EDINBURGH", "LEEDS", Colour.BLUE);
            sm.InsertConnection("LEEDS","LONDON", Colour.BLUE);
            sm.InsertConnection("LONDON","OXFORD", Colour.BLUE);
            sm.InsertConnection("OXFORD","NEWBURY", Colour.BLUE);
            sm.InsertConnection("NEWBURY","WINCHESTER", Colour.BLUE);
            sm.InsertConnection( "WINCHESTER","MALBOROUGH", Colour.BLUE);
            sm.InsertConnection("MALBOROUGH","WARMINISTER", Colour.BLUE);
            sm.InsertConnection("WARMINISTER","BOURNEMOUTH", Colour.BLUE);

            // YELLOW LINE ==> CONNECTIONS
            sm.InsertConnection("HULL","COLCHESTER",Colour.YELLOW);
            sm.InsertConnection("COLCHESTER","OSTERLY", Colour.YELLOW);
            sm.InsertConnection("OSTERLY","HEATHROW", Colour.YELLOW);
            sm.InsertConnection("HEATHROW","READING", Colour.YELLOW);
            sm.InsertConnection("READING","WINCHESTER", Colour.YELLOW);
            sm.InsertConnection("WINCHESTER","BATH", Colour.YELLOW);
            sm.InsertConnection( "BATH","BRISTOL", Colour.YELLOW);
            sm.InsertConnection("BRISTOL","CARDIFF", Colour.YELLOW);


            //GREY LINE ==> CONNECTIONS
            sm.InsertConnection("PETERBOROUGH","IPSWICH",Colour.GREY);
            sm.InsertConnection( "IPSWICH","SUDBURY", Colour.GREY);
            sm.InsertConnection("SUDBURY","CLEMSFORD",Colour.GREY);
            sm.InsertConnection("CLEMSFORD", "HEATHROW", Colour.GREY);
            sm.InsertConnection("HEATHROW","NEWBURY", Colour.GREY);
            sm.InsertConnection( "NEWBURY","SWINDON", Colour.GREY);
            sm.InsertConnection("SWINDON", "BATH", Colour.GREY);
            sm.InsertConnection("BATH", "BRISTOL", Colour.GREY);
            sm.InsertConnection("BRISTOL", "CARDIFF", Colour.GREY);

            // Test Case 1
            Console.WriteLine("===========");

            Console.WriteLine("Finding the route from 'Liverpool' to 'Newbury'");
            sm.SRoute("LIVERPOOL", "NEWBURY");

            // Test Case 2
            Console.WriteLine("===========");

            Console.WriteLine("Finding the route from 'Cardiff' to 'Ipswich'");
            sm.SRoute("CARDIFF","IPSWICH");

            // Test Case 3
            Console.WriteLine("===========");

            //sm.RemoveStation("LONDON");
            //Console.WriteLine("Finding the route from 'Cardiff' to 'Leeds'");
            //sm.SRoute("CARDIFF", "LEEDS");

            

            // Test Case 4
            Console.WriteLine("===========");

            Console.WriteLine("Removing conection from 'Manchester' and 'Clifton'");
            bool remove1 = sm.RemoveConnection("MANCHESTER", "CLIFTON", Colour.RED);
            if (remove1 == false)
            {
                Console.WriteLine("No connection Found");
            }
            else
            {
                Console.WriteLine("Connection Removed");
            }

            Console.WriteLine("Finding the route from 'Manchester' to 'Ipswich'");
            sm.SRoute("MANCHESTER", "IPSWICH");
            Console.WriteLine("===========");

            //Test Case 5
            Console.WriteLine("Finding the route from 'London' to 'Peterborough’");
            sm.SRoute("LONDON", "PETERBOROUGH");









        }
    }
}
