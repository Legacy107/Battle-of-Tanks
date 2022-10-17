using System.Collections.Generic;
using System.Linq;

namespace BattleOfTanks
{
    public class CommandExecutor
    {
        private List<EffectCommand> _commands;
        private static CommandExecutor? _instance;

        private CommandExecutor()
        {
            _commands = new List<EffectCommand>();
        }

        public void AddCommand(EffectCommand newCommand)
        {
            // Uniqe effect can only appear one in the queue for each subject
            if (newCommand.Unique)
                foreach (EffectCommand command in _commands)
                    if (newCommand.Equals(command))
                    {
                        // Renew the duration
                        command.Duration = newCommand.Duration;
                        return;
                    }

            _commands.Add(newCommand);
        }

        public void Execute(double delta)
        {
            // Execute in the order of insertion
            foreach (EffectCommand command in _commands)
                command.Execute();

            // Remove in reverse order to avoid index issue
            // https://stackoverflow.com/questions/1582285/how-to-remove-elements-from-a-generic-list-while-iterating-over-it
            foreach (EffectCommand command in _commands.Reverse<EffectCommand>())
            {
                command.Duration -= delta;
                if (command.Duration < 0.0)
                {
                    command.OnRemove();
                    _commands.Remove(command);
                }
            }
        }

        public static CommandExecutor Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new CommandExecutor();

                return _instance;
            }
        }
    }
}

