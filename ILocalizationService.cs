﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameGum.Localization;
public interface ILocalizationService
{
    string Translate(string stringId);
}
