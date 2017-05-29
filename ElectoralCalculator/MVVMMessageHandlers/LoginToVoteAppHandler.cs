using ElectoralCalculator.MVVMMessages;
using ElectoralCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.MVVMMessageHandlers
{
    public class LoginToVoteAppHandler : MessageHandlerBase<LoginToVoteAppMessage>
    {
        private static VoterLoginViewModel _viewModel;

        public LoginToVoteAppHandler() : base()
        {
            MVVMMessagerService.RegisterReceiver<LoginToVoteAppMessage>(Handle);
        }

        public static void SetViewModel(VoterLoginViewModel viewModel)
            => _viewModel = viewModel;

        public override async void Handle(LoginToVoteAppMessage obj)
        {
            await _viewModel.ValidateUserAsync();
        }
    }
}
