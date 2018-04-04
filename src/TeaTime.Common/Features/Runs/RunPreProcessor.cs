﻿namespace TeaTime.Common.Features.Runs
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using MediatR.Pipeline;

    public class RunPreProcessor<TCommand> : IRequestPreProcessor<TCommand>
    {
        private readonly RunLockProcessor _runLockProcessor;

        public RunPreProcessor(RunLockProcessor runLockProcessor)
        {
            _runLockProcessor = runLockProcessor;
        }

        public Task Process(EndRunCommand request, CancellationToken cancellationToken)
        {
            if(!request.Orders.Any())
                throw new Exception("Run cannot start without any orders"); //todo: Exception

            if(!request.Orders.Any(o => o.UserId.Equals(request.UserId)))
                throw new Exception("Cannot end run without joining first"); //todo: Exception

            return Task.CompletedTask;
        }

        public async Task Process(TCommand request, CancellationToken cancellationToken)
        {
            if (request is EndRunCommand endRunCommand)
            {
                await Process(endRunCommand, cancellationToken).ConfigureAwait(false);
                await _runLockProcessor.Process(endRunCommand, cancellationToken).ConfigureAwait(false);
                return;
            }

            if (request is StartRunCommand startRunCommand)
            {
                await _runLockProcessor.Process(startRunCommand, cancellationToken).ConfigureAwait(false);
                return;
            }
        }
    }
}
