﻿namespace Recallio.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
  
    (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
}