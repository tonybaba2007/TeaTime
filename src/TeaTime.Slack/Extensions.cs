﻿namespace TeaTime.Slack
{
    using Common.Abstractions;
    using Models;
    using Models.Requests;
    using Models.Requests.InteractiveMessages;

    internal static class Extensions
    {
        internal static void AddCallbackState(this BaseCommand command, CallbackData callbackData)
        {
            command.State[Constants.CallbackData] = SlackJsonSerializer.Serialize(callbackData);
        }

        internal static bool TryGetCallbackState(this Event command, out CallbackData callbackData)
        {
            if (!command.State.TryGetValue(Constants.CallbackData, out var json))
            {
                callbackData = null;
                return false;
            }

            try
            {
                callbackData = SlackJsonSerializer.Deserialize<CallbackData>(json);
                return callbackData != null;
            }
            catch
            {
                callbackData = null;
                return false;
            }
        }

        internal static CallbackData ToCallbackData(this SlashCommand command)
        {
            return new CallbackData
            {
                UserId = command.UserId,
                ResponseUrl = command.ResponseUrl
            };
        }

        internal static CallbackData ToCallbackData(this MessageRequestPayload payload)
        {
            return new CallbackData
            {
                UserId = payload.User.Id,
                ResponseUrl = payload.ResponseUrl
            };
        }
    }
}
