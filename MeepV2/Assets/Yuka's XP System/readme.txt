while its not required, credit would be much appreciated if you use this in your project!

(HOW TO SET UP)
1: drag the prefab into the scene
2: the script should already be set up, if not, set it up
3: thats it

(HOW TO IMPLEMENT INTO YOUR COPY)

your creativity is the only limit here, but for example, if you want the player to gain 1000 XP for tagging a player, put this line in GorillaTagger.cs where the tagging happens
FindFirstObjectByType<XP_Manager>().AddXp(1000);

again, you can go wild with this.