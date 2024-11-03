# Checkout Champion
Bagging at a grocery store can be a difficult job. Meet wacky customers, and do your best to not drop any groceries! See how you stack up in Checkout Champion! 

![Checkout Champion Title Screen](https://imgur.com/a/hxiP9A2)

## How to play
Download the latest release from the release tab on the right-side of the page.
Extract the .zip file, and run the .exe file.
Bag some groceries!

## Inspiration
We were inspired to create games about less popular jobs within the farm-to-table chain.  There are many farming or cooking games out there, but not too many about bagging groceries.  Our initial inspiration was a tetris-like game, but we shifted to using Unity Physics to pursue that chaotic fun.

## What it does
Our game has a conveyor belt that constantly feeds you grocery items.  As the bagger, it is your job to pack all the items into a crate without missing anything.  Any items that fall out when you put the crate in the grocery cart lose you points. At the end of each day, you may spend your hard-earned cash on decorations that sit proudly on the shelf in front of you.

## How we built it
We built our game using Unity and C#.  We used Blender for animations, Paint 3D and Krita for art, Soundtrap for music, Github for source control, and Trello for organization. All code was written by us during the event, but several free 3D assets were sourced online.

## Challenges we ran into
Getting Unity and Github to play nice is always difficult, but a solution that (mostly) worked for us was to create a unique scene for each new feature and copy features into the final scene after merging. We found that scene files are the most common thing that break when merging with a Unity project because they aren't formatted in a way that allows them to combine changes the way Github wants to, and this allowed us to work around that limitation. When working with physics, there are also a lot of little things that are important to account for to keep things from behaving strangely or just feeling "off."

## Accomplishments that we're proud of
One feature that we were excited to include was a dynamic soundtrack, with tracks that combine in unique ways based on the items being bagged. We also reached a level of polish we're proud of in our game: things happen smoothly and feel good.

## What we learned
Moreso than any one trick, we all improved quite a bit at working with Unity and learning the ins and outs of the engine. The audio system was something we particularly learned a lot about while making the dynamic soundtrack, but also the animation system and Blender-to-Unity workflows and the intricacies of rigidbody physics. We learned project management skills as we planned out the best way to divide up tasks that would combine into the final product.

## What's next for Checkout Champion
We'd like to improve and expand the core gameplay loop. Different crate shapes and sizes? More customers and decorations? We'd love to think outside the box on better ways for you to put things *inside* the box.