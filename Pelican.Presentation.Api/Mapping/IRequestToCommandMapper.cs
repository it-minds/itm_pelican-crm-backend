﻿using Pelican.Application.Abstractions.Messaging;
using Pelican.Presentation.Api.Contracts;

namespace Pelican.Presentation.Api.Mapping;

internal interface IRequestToCommandMapper
{
	IReadOnlyCollection<ICommand> ConvertToCommands(IReadOnlyCollection<WebHookRequest>? requests);
}