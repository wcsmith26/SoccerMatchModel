import pandas as pd
import networkx as nx

def GenerateGraph(filename):
    #Reading the collected data into a dataframe.
    df = pd.read_csv(filename)

    #Initializing the graph
    G = nx.DiGraph(directed=True)
    G.add_nodes_from(df.Name)
    G.add_node('Turnover')
    G.add_node('ShotOn')
    G.add_node('ShotOff') #ShotOff will be total-shotson from dataframe

    #Loops over all players in the dataframe
    for i in range(11):
        #Generates the edge to the turnover, with weight taken from the dataframe
        if (df.iloc[i][i+1] > 0):
            G.add_edge(df.Name[i], 'Turnover', weight=(df.iloc[i][i+1]))
        #Generates the edges to the shoton and shotoff nodes
        if (df.iloc[i][13] > 0 or df.iloc[i][14] > 0):
            shoton = df.iloc[i][13]
            shotoff = df.iloc[i][14] - shoton
            G.add_edge(df.Name[i], 'ShotOn', weight=shoton)
            G.add_edge(df.Name[i], 'ShotOff', weight=shotoff)

        #Generates the edges to all other players, with weight taken from the dataframe
        for j in range(11):
            if (i == j): continue
            if (df.iloc[i][j+1] > 0):
                G.add_edge(df.Name[i], df.Name[j], weight=(df.iloc[i][j+1]))

    return G