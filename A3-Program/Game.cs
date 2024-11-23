namespace COIS2020.MolayoOgunfowora0772346.Assignment3;

using Microsoft.Xna.Framework; // Needed for Vector2
using TrentCOIS.Tools.Visualization;
using COIS2020.StarterCode.Assignment3;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Linq;
using System.Collections.Generic;

public class CastleDefender : Visualization
{
    public LinkedList<Wizard> WizardSquad { get; private set; }
    public Queue<Wizard> RecoveryQueue { get; private set; }

    public LinkedList<Goblin> GoblinSquad { get; private set; }
    public LinkedList<Goblin> BackupGoblins { get; private set; }

    public LinkedList<Spell> Spells { get; private set; }
    public Node<Wizard>? ActiveWizard { get; private set; }

    private uint nextSpellTime;
    private Vector2 goblinDirection;


    public CastleDefender()
    {
        Random rand= new Random();
        WizardSquad = new();
        GoblinSquad = new();
        BackupGoblins = new();
        Spells = new();
        RecoveryQueue = new();

        for (int i =0; i<8; i++)
        {
            WizardSquad.AddBack(new Wizard()); //Add to wizard squad
            GoblinSquad.AddBack(new Goblin()); //Add to GoblinSquad
        }

        for (int i = 0; i < 6; i++)
        {
            BackupGoblins.AddBack(new Goblin()); //Add to wizard squad
        }

        if(WizardSquad.Head != null)
        {
            ActiveWizard = WizardSquad.Head;
        }
        float y = (float)(rand.NextDouble()); // Generate a random value between 0 and 1
        float x = (float) Math.Sqrt(1 -(y * y)); //determine it's corresponding x value for vector to be a magnitude of 1
        nextSpellTime = (uint)rand.Next(10, 20);
        goblinDirection = new Vector2(x, y);
        goblinDirection.Normalize();

    }


    protected override void Update(uint currentFrame)
    {
        UpdateSpells();
        UpdateGoblins();
        UpdateWizards();
        CallBackup();
        PauseGame();
    }


    public void UpdateSpells()
    {
        Vector2 spellDirection = new Vector2(0, -5); //(negative Y-direction)
        var current = Spells.Head;

        //iterate through the spells list
        while (current != null)
        {
            var spell = current.Item;

            // Move the spell upwards
            spell.MoveTowards(spellDirection, Spell.Speed);
            Node <Spell> temp = current.Next;
            // Check if the spell is off the screen
            if (CastleGameRenderer.IsOffScreen(spell))
            {
                // Remove the spell from the list
                Spells.Remove(current);
            }

            current = temp;
        }
    }

    public void UpdateGoblins()
    {
        Random rand = new Random();
        float y = (float)(rand.NextDouble()); // Generate a random value between 0 and 1
        float x = (float)Math.Sqrt(1 - (y * y)); //determine it's corresponding x value for vector to be a magnitude of 1
        //Declare Tail Goblin
        Node<Goblin> currentGoblin = GoblinSquad.Tail;

        //Loop to move every other Goblin
        while (currentGoblin != GoblinSquad.Head)
        {
            //Move Goblin
            currentGoblin.Item.MoveTowards(currentGoblin.Prev.Item, Goblin.Speed);
            currentGoblin = currentGoblin.Prev;
        }
        // Move Head Goblin
        GoblinSquad.Head.Item.Move(goblinDirection, Goblin.Speed);

        //check for collision
        CastleGameRenderer.CheckWallCollision(GoblinSquad.Head.Item, ref goblinDirection);

        //Code to detect spell collision

        //Redeclare currentGoblin and currentSpell as Head
        currentGoblin = GoblinSquad.Head;
        
        //Loop through goblins
        while (currentGoblin != null)
        {
            Node<Goblin> goblinTemp = currentGoblin.Next;
            Node<Spell> currentSpell = Spells.Head;
            //Loop through spells
            while (currentSpell != null)
            {
                Node<Spell> spellTemp = currentSpell.Next;
                if (currentGoblin.Item.Colliding(currentSpell.Item))
                {
                    //remove objects from respective lists
                    GoblinSquad.Remove(currentGoblin);
                    Spells.Remove(currentSpell);

                    //Declare a new direction for Lead Goblin
                    Vector2 newGoblinDirection = new Vector2(x, y); //need to change directions
                    goblinDirection = newGoblinDirection;
                    if (GoblinSquad.Head != null)
                    {
                        GoblinSquad.Head.Item.Move(newGoblinDirection, Goblin.Speed);
                        //Console.WriteLine("New Direction: " + newGoblinDirection);
                    }
                    
                }
                //increment spell
                currentSpell = spellTemp;
            }
            //increment goblin
            currentGoblin = goblinTemp;
        }

    }

    public void UpdateWizards()
    {
        Random rand = new Random();
        if (nextSpellTime <= 0 && ActiveWizard != null)
        {
            //Cast a Spell
            Spell newSpell = new Spell(ActiveWizard.Item.SpellType, ActiveWizard.Item.Position);
            Spells.AddBack(newSpell);

            //decrement energy level
            ActiveWizard.Item.Energy -= ActiveWizard.Item.SpellLevel;

            //move to next Wizard
            if(ActiveWizard.Next!= null)
            {
                ActiveWizard = ActiveWizard.Next;
            }
            else
            {
                ActiveWizard= WizardSquad.Head;
            }
            //RESET timer
            nextSpellTime = (uint)rand.Next(10, 20);
        }
        else
        {
            //decrement timer
            nextSpellTime--;
        }

        //Determine if wizard has enough energy
        while (ActiveWizard != null && ActiveWizard.Item.Energy <= 0)
        {
            //remove from wizardsquad
            WizardSquad.Remove(ActiveWizard);
            //Add to recovery queue
            RecoveryQueue.Enqueue(ActiveWizard.Item);
            //set new activeWizard
            ActiveWizard = ActiveWizard.Next ?? WizardSquad.Head;
        }

        //Determine if any Wizard in queue
        if(!RecoveryQueue.IsEmpty)
        {
            Wizard currentWizard = RecoveryQueue.Peek();

            //Replenish energy every 5 frames
            if(nextSpellTime % 5 == 0)
            {
                currentWizard.Energy += 1;
            }

            //determine if wizard has recovered

            if(currentWizard.Energy >= currentWizard.MaxEnergy)
            {
                Wizard recoveredWizard = RecoveryQueue.Dequeue();
                if(ActiveWizard!= null)
                {
                    WizardSquad.InsertBefore(ActiveWizard, recoveredWizard);
                }
                else
                {
                    WizardSquad.AddBack(recoveredWizard);
                }
                
            }
         
        }
    }

    public void CallBackup()
    {
        if (GoblinSquad.getCount() <= 4)
        {
            GoblinSquad.AppendAll(BackupGoblins);
        }
    }

    public void PauseGame()
    {
        if (GoblinSquad.getCount() == 0)
        {
            Pause();
            Console.WriteLine("Game has finished");
        }
    }

}
