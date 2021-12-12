namespace Kamek.Hooks
{
    abstract class Hook
    {
        public static Hook Create(Linker.HookData data, AddressMapper mapper)
        {
            return data.type switch
            {
                1 => new WriteHook(false, data.args, mapper),
                2 => new WriteHook(true, data.args, mapper),
                3 => new BranchHook(false, data.args, mapper),
                4 => new BranchHook(true, data.args, mapper),
                5 => new PatchExitHook(data.args, mapper),
                _ => throw new NotImplementedException("unknown command type"),
            };
        }


        public readonly List<Commands.Command> Commands = new();

        protected Word GetValueArg(Word word)
        {
            // _MUST_ be a value
            if (word.Type != WordType.Value)
                throw new InvalidDataException(string.Format("hook {0} requested a value argument, but got {1}", this, word));

            return word;
        }

        protected Word GetAbsoluteArg(Word word, AddressMapper mapper)
        {
            if (word.Type != WordType.AbsoluteAddr)
            {
                if (word.Type == WordType.Value)
                    return new Word(WordType.AbsoluteAddr, mapper.Remap(word.Value));
                else
                    throw new InvalidDataException(string.Format("hook {0} requested an absolute address argument, but got {1}", this, word));
            }

            return word;
        }

        protected static Word GetAnyPointerArg(Word word, AddressMapper mapper)
        {
            return word.Type switch
            {
                WordType.Value => new Word(WordType.AbsoluteAddr, mapper.Remap(word.Value)),
                WordType.AbsoluteAddr or WordType.RelativeAddr => word,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
