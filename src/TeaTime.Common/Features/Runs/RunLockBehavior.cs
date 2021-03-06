﻿namespace TeaTime.Common.Features.Runs
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using Commands;
    using Exceptions;
    using MediatR;

    public class RunLockBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
    {
        private readonly IRoomRunLockService _lockService;

        public RunLockBehavior(IRoomRunLockService lockService)
        {
            _lockService = lockService;
        }

        public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await ProcessAsync(request, cancellationToken);

            return await next();
        }

        private Task ProcessAsync(TCommand request, CancellationToken cancellationToken)
        {
            switch (request)
            {
                case StartRunCommand startRunCommand:
                    return ProcessAsync(startRunCommand, cancellationToken);
                case EndRunCommand endRunCommand:
                    return ProcessAsync(endRunCommand, cancellationToken);
            }

            return Task.CompletedTask;
        }

        private async Task ProcessAsync(StartRunCommand request, CancellationToken cancellationToken)
        {
            //try to create the run lock
            var created = await _lockService.CreateLockAsync(request.RoomId).ConfigureAwait(false);
            if (!created)
                throw new RunStartException("There is already an active run in this room", RunStartException.RunStartExceptionReason.ExistingActiveRun);

            //run lock created, so everything is ok
        }

        private async Task ProcessAsync(EndRunCommand request, CancellationToken cancellationToken)
        {
            //delete lock
            var deleted = await _lockService.DeleteLockAsync(request.RoomId).ConfigureAwait(false);
            if (!deleted)
                throw new RunEndException("There is no active run in this room", RunEndException.RunEndExceptionReason.NoActiveRun);
        }
    }
}
