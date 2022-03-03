import pandas as pd
import networkx as nx
import random as rd
import time
from PassingDataModel import GenerateGraph

G1 = GenerateGraph('Chelsea11-28.csv')

#Code below verifies that our graph was initialized correctly.
print('Mendy turned the ball over', G1['Mendy']['Turnover']['weight'], 'time(s).\n')
print('Mendy passed to Rudiger', G1['Mendy']['Rudiger']['weight'], 'time(s).\n')
print('ShotsOff for Alonso:', G1['Alonso']['ShotOff']['weight'])

#Code Organizing and drawing the Graph, so that it looks more like a soccer formation.

#Hardcoding the positions to represent Chelsea's formation in this game.
pos = {'Mendy': (0,3), 'Alonso': (1.5,1), 'Rudiger': (1,2), 'ThiagoSilva': (.75,3), 'Chalobah': (1,4), 'James': (1.5,5), 'Jorghino': (2,2), 'Loftus-Cheek': (2,4), 'Hudson-Odoi': (3.5,1.25), 'Ziyech': (3.5,4.75), 'Werner': (4.25,3.1), 'Turnover': (5.5,5), 'ShotOn': (6,3), 'ShotOff': (5.5,1)}

#Initializing the colors of the nodes
color_map = []
for node in G1:
    if node == 'Turnover' or node == 'ShotOn' or node == 'ShotOff':
        color_map.append('red')
    else:
        color_map.append('blue')

nx.draw(G1, pos, node_color=color_map, with_labels=True)

#Makes a single play based from a node within the graph G. The play is made by semi-randomly selecting an outgoing edge based on the weights.
def PlayOne(G, node):
    edges=G.out_edges(node)
    dest=[]
    for e in edges:
        for i in range(G[node][e[1]]['weight']):
            dest.append(e[1]) #Adds the destination node to the list we will choose from weight times. So randomly selecting from the list will be reflective of the weights of the edges.

    return rd.choice(dest)

print(PlayOne(G1, 'Mendy'))

def SimulatePossession(G, node):
    queue = []
    queue.append(node)
    G_default = G
    G_colored = G
    x = node
    prev = node
    while (x != 'Turnover' and x != 'ShotOn' and x != 'ShotOff'):
        prev = x
        x = PlayOne(G, prev)
        queue.append(x)
    return queue

SimulatePossession(G1, 'Werner')

def VisualizePossession(queue, G, positions, colors):
    G_new = nx.DiGraph(directed=True)
    G_new.add_nodes_from(G.nodes())
    for i in range(len(queue)-1):
        G_new.add_edge(queue[i], queue[i+1])
    nx.draw(G_new, positions, node_color=colors, with_labels=True)
    
def PrintPossession(queue):
    num_items = len(queue)
    for i in range(num_items-2):
        print(queue[i], "passes to", queue[i+1] + ".")
        # time.sleep(.75) #COMMENT OUT FOR QUICKER BUT LESS ENGAGING SIMULATIONS
    if (queue[num_items-1] == 'Turnover'):
        print(queue[num_items-2], "turns the ball over.\n")
    elif (queue[num_items-1] == 'ShotOn'):
        print(queue[num_items-2], "shoots on target.")
    else:
        print(queue[num_items-2], "shoots off target.\n")
        
queue = SimulatePossession(G1, 'Mendy')
PrintPossession(queue)
VisualizePossession(queue, G1, pos, color_map)

def SimulateGame(team1, team2, num_pos):
    score1 = 0
    score2 = 0
    index = 0
    t1_nodes = list(team1.nodes())
    t2_nodes = list(team2.nodes())
    queue = []
    n=0
    while (n < num_pos):
        queue = SimulatePossession(team1, t1_nodes[index])
        if(queue[len(queue)-1] == 'ShotOn'):
            if (rd.choice(['ShotOn', 'ShotOn', 'ShotOn', 'ShotOn', 'Goal']) == 'Goal'):
                print("Team 1 Possession:")
                PrintPossession(queue)
                print(queue[len(queue)-2], "scores a goal!\n")
                score1 += 1
                index = t1_nodes.index(queue[len(queue)-2])
                print("Team 1:", score1, "Team 2:", score2,"\n")
                
        queue = SimulatePossession(team2, t2_nodes[index])
        if(queue[len(queue)-1] == 'ShotOn'):
            if (rd.choice(['ShotOn', 'ShotOn', 'ShotOn', 'ShotOn', 'Goal']) == 'Goal'):
                print("Team 2 Possession:")
                PrintPossession(queue)
                print(queue[len(queue)-2], "scores a goal!\n")
                score2 += 1
                index = t2_nodes.index(queue[len(queue)-2])
                print("Team 1:", score1, "Team 2:", score2,"\n")
        n += 1
    print("FINAL SCORE:\n", "Team 1:", score1, "Team 2:", score2)
SimulateGame(G1, G1, 100)

ex_filename1 = 'Exercise.csv'
# ex_filename2 = 'PUT FILE NAME HERE'

G2 = GenerateGraph(ex_filename1)
# G3 = GenerateGraph(ex_filename2)

pos2 = {'Donnarumma': (0,3), 'Hakimi': (1.5,1), 'Marquinhos': (1,2), 'Ramos': (.75,3), 'Diallo': (1,4), 'Bernat': (1.5,5), 'Wijnaldum': (2,2), 'Verrati': (2,4), 'Neymar': (3.5,1.25), 'Mbappe': (3.5,4.75), 'Messi': (4.25,3.1), 'Turnover': (5.5,5), 'ShotOn': (6,3), 'ShotOff': (5.5,1)}

SimulateGame(G1, G2, 100)