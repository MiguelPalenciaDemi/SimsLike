# Sims-Like Needs System Prototype

This is a prototype inspired by *The Sims* to experiment with needs management and character autonomy. The character has basic needs and can interact with objects to satisfy them, choosing its actions semi-autonomously.

## 🎮 Features.
- **Needs management**: Hunger, energy, fun, hygiene, social and bladder.
- **AI system**: The character analyzes available objects, evaluates its options and selects an action based on a dynamic priority system.
- **Flexible decision making**: It does not always choose the most obvious action, but one of the three best options, avoiding predictable behaviors.
- **Autonomy**: If the player does not assign tasks, the character will act on its own.

## 🧠 How does the AI work?
1. The character requests a list of interacting objects from the house.
2. Evaluates what actions are available on each object.
3. Each action is weighted according to the current needs of the character, applying dynamic weights (inspired by *The Sims* and adjusted with **AnimCurves**).
4. Some actions can satisfy more than one need at a time (e.g. playing on the computer with friends improves **fun** and **social**).
5. A ranking of actions is generated and the character selects one of the three best options, achieving a more natural behavior.


## 📥 Installation
1. Clone this repository:
   ```sh
   git clone https://github.com/MiguelPalenciaDemi/SimsLike.git
   ```
2. Open the project in Unity (recommended version: 6000.0.32f1).
3. Run the main scene and experiment with the AI.

## 🚀 Future goals
- Improve the variety of interactions.
- Refine decision making to make it more sophisticated.
- Implement animations and visual improvements.

## 📜 Credits.
Inspired by *The Sims*, developed by **Maxis/EA**, a game that marked me and motivated me to dedicate myself to video game development.

## 📢 Contact
If you have comments or suggestions, I'd love to hear them!
- BlueSky: [https://bsky.app/profile/miguelpalencia.bsky.social](https://bsky.app/profile/miguelpalencia.bsky.social)
- Twitter: [@MiguelPalenciaD](https://twitter.com/MiguelPalenciaD)
- Itch.io: [MiguelPalenciaItchio](https://miguelpalenciademi.itch.io)
- LinkedIn: [Miguel](https://linkedin.com/in/MiguelPalenciadm)
