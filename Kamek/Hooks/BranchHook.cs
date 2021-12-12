namespace Kamek.Hooks
{
    class BranchHook : Hook
    {
        public BranchHook(bool isLink, Word[] args, AddressMapper mapper)
        {
            if (args.Length != 2)
                throw new InvalidDataException("wrong arg count for BranchCommand");

            // expected args:
            //   source : pointer to game code
            //   dest   : pointer to game code or to Kamek code
            var source = GetAbsoluteArg(args[0], mapper);
            var dest = GetAnyPointerArg(args[1], mapper);

            Commands.Add(new Commands.BranchCommand(source, dest, isLink));
        }
    }
}
