using System;
using UnityEngine;

public class EventHandlerExample : MonoBehaviour
{
    /***
     * Some important notes on usage and extension of this style of Event Based Architecture/Programming in Unity/C#
     *
     * 1. Why use this? - This is an incredible way of decoupling your Unity Components. In the example below
     *      we use a static EventHandler that can be subscribed to at any time without a reference to a specific Instance
     *      or GameObject/MonoBehaviour making it easier for you to manage large systems without maintaining explicit references.
     *      For example using below in a traditional method, the Enemy class would have to know about the ScoreBoard and explicitly
     *      tell the ScoreBoard when it is hit so the points can be added. In This example, Enemy does not need to know ScoreBoard
     *      exists at all, it's sole responsibility is raising the EventHandler - Fire and Forget. ScoreBoard can register
     *      or de-register at any time, and so can any other listener without impacting the behaviour of the Enemy class.
     * 
     * 2. Configurable EventHandler args - EventHandlers do not need to pass the class they are part of.
     *      For the example below we are implementing the event with class Enemy and passing self.
     *      You could alternatively pass a custom OnHitEventArgs class that implements the Enemy hit and
     *      Player that hit the enemy. 
     *
     * 3. Remove EventHandler subscribers - Objects that get deleted do not get removed
     *      from EventHandlers. You must manually remove them using the (-=) method
     *      this is typically done when a MonoBehaviour that was listening to an EventHandler (+=) is Destroyed.
     *
     * 4. Static vs Instance events - The example below uses a Static event, subscribable by any object at any time.
     *      This is great from the context of the ScoreBoard as it needs to know about every enemy hit. You can instead
     *      use Instance events when you want to only know about an event on specific object (Instance).
     *
     * 5. event keyword vs EventHandler - There is a difference, and as you look online you will see many different
     *      implementations of the below. You'll notice this example does not use the event keyword. A great overview
     *      of the differences and potential drawbacks can be found here:
     *      https://stackoverflow.com/questions/46824524/event-vs-eventhandler
     *      TLDR: This method is easier to implement, though not as safe and adds functionality for multiple listeners.
     */
    

    /// Psuedo player class. Not strictly necessary for the Event, just shows how you would kick off the chain of events. 
    public class Player 
    {
        public void Shoot()
        {
            // Insert real code here to get the enemy hit
            // In the meantime create a fake enemy to hit
            Enemy enemy = new Enemy();
            enemy.Hit();
        }
    }

    // Enemy is our example for the source of an Event. In this case when an Enemy is hit, it will raise on OnHit event.
    public class Enemy
    {
        // Event Declaration
        public static EventHandler<Enemy> OnHit;
        
        public void Hit()
        {
            // Raise the OnHit Event - notifying all static subscribers and passing self.
            if (OnHit != null)
                OnHit(this, this);
        }
    }

    
    // ScoreBoard is our example for a listener/subscriber to our Enemy's OnHit event
    public class ScoreBoard
    {
        private int _enemiesHit = 0;
        
        public ScoreBoard()
        {
            // Subscribe to the OnHit static Event with our OnEnemyHit method as the subscriber
            Enemy.OnHit += OnEnemyHit;
        }

        // Our OnEnemyHit method that accepts two params.
        // EventHandler always passes `object` type as the first arg and the T type as the second
        private void OnEnemyHit(object sender, Enemy enemy)
        {
            _enemiesHit++;
            Debug.Log($"{_enemiesHit} enemies have been hit!");
        }
    }
}