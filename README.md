# HSGA
Dissertation Project COMPLETE - HearthStone Genetic Algorithm for generating decks.

After months of toiling away, the HSGA is now (for the time being) complete. 

The HSGA, or Hearthstone Genetic Algorithm, has the ability to generate Hearthstone decks for any class, up to the Mean Streets of Gadgetzan expansion. (A limitation due to Metastone not being updated further).
Alongside the algorithm is Metastone, an open-source implementation created by [demilich1](https://github.com/demilich1/metastone). This was used as the test bed for the decks.

During my study, I performed testing on 4 seperate classes:

#### Shaman
The Shaman class was tested with the following modifiers:

* Population size of 10.
* Mutation Rate of 0.5, that steadily decreases over time.
* Single Point Crossover, with a crossover rate of 0.7.
* Maximum generation count of 50.
* Tournament pool size of 4.

Results showed that the final generated deck was completely filled with neutral minion cards, with a mana curve weighting towards aggro and midrange archetypes.
Play-by-play analysis showed that the deck performed poorly, both due to the composition of the deck, and the AI implemented within Metastone.

#### Warlock
The Warlock class was tested with the following modifiers:

* Population size of 10.
* Mutation Rate of 0.5, that steadily decreases over time.
* Single Point Crossover, with a crossover rate of 0.7.
* Maximum generation count of 50.
* Tournament pool size of 2.

Results showed that the final generated deck again was filled with neutral minion cards, although the mana curve pointed towards the zoo archetype.
Play-by-play analysis showed better performance than the Shaman tests, as the Warlock managed to win against a human created deck.

#### Mage
The Mage class was tested with the following modifiers:

* Population size of 10.
* Mutation Rate of 0.02.
* Two Point Crossover, with a crossover rate of 0.7.
* Maximum generation count of 10.
* Tournament pool size of 4.

Results showed that the Mage deck had improvements over the last tests, in that the deck consisted of class and spell cards, instead of just neutral minions.
However, the play-by-play analysis showed the Mage deck performing poorly, as the composition of the deck was not well suited for the average Mage playstyle, i.e. high usage of spell cards.

#### Paladin
The Paladin class was tested with the following modifiers:

* Population size of 10.
* Mutation Rate of 0.02.
* Single Point Crossover, with a crossover rate of 0.7.
* Maximum generation count of 10.
* Tournament pool size of 6.

Results showed that the Paladin deck had significant improvements over all the other decks, with a good mix of neutral, class and spell cards. Play-by-play analysis also showed improvements, as the deck managed to win against the AI in a similar fashion to a human player might play.

In its current state, it is unusable outside of my home system (I blame deadlines for that). However, I plan to refactor it to a state where it is usuable by anyone.

