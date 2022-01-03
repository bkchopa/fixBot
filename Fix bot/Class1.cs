using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System.Collections.Generic;

namespace Fix_bot
{
    // 모듈 클래스의 접근제어자가 public이면서 ModuleBase를 상속해야 모듈이 인식된다.
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        static List<string> waitList = new List<string>();
  
        /// <summary>
        /// !hi 명령어를 입력했을 때 실행되는 함수
        /// </summary>
        [Command("체크")]
        [Alias("추가")]
        public async Task HelloCommand(string _message)
        {
            if (waitList.Contains(_message))
                return;


            waitList.Add(_message);
            //ModuleBase를 상속하면 Context 변수를 통해 답장을 보낼 수 있다. 
            //await Context.Channel.SendMessageAsync("추가됨 : " + _message);
        }

        [Command("양보")]
        public async Task YangBoCommand(string _message)
        {
            if (waitList.Contains(_message))
                return;

            waitList.Insert(0, _message);
            //ModuleBase를 상속하면 Context 변수를 통해 답장을 보낼 수 있다. 
            //await Context.Channel.SendMessageAsync("양보 됨 : " + _message);
        }

        [Command("취소")]
        public async Task ByeCommand(string _message)
        {
            int i = 0;
            bool result = int.TryParse(_message, out i);
            if(result)
            {
                if(i <= 0 || i> waitList.Count)
                {
                    await Context.Channel.SendMessageAsync("잘못된 번호입니다 : " + i);
                    return;
                }
                string temp = waitList[i - 1];
                waitList.RemoveAt(i - 1);
                //await Context.Channel.SendMessageAsync("취소됨 : " + temp);
                string tempRet = new string("");
                for (int k = 0; k < waitList.Count; k++)
                {
                    tempRet += (k + 1).ToString() + ". ";
                    tempRet += waitList[k];
                    tempRet += "\0";
                }


                await Context.Channel.SendMessageAsync(tempRet);

                return;
            }


            if (!waitList.Contains(_message))
            {
                await Context.Channel.SendMessageAsync("대기명단에 없는 닉네임입니다");
                return;
            }

            //ModuleBase를 상속하면 Context 변수를 통해 답장을 보낼 수 있다. 
            waitList.Remove(_message);
            //await Context.Channel.SendMessageAsync("취소됨 : " + _message);
            string ret = new string("");
            for (int j = 0; j < waitList.Count; j++)
            {
                ret += (j + 1).ToString() + ". ";
                ret += waitList[j];
                ret += "\0";
            }


            //await Context.Channel.SendMessageAsync(ret);
        }

        [Command("대기")]
        public async Task ListCommand()
        {
            if(waitList.Count ==0)
            {
                await Context.Channel.SendMessageAsync("현재 대기 없음");
                return;
            }

            //ModuleBase를 상속하면 Context 변수를 통해 답장을 보낼 수 있다. 
            string ret = new string("");
            for(int i =0; i< waitList.Count; i++)
            {
                ret += (i + 1).ToString() + ". ";
                ret += waitList[i];
                ret += "\0";
            }


            await Context.Channel.SendMessageAsync(ret);
        }

        [Command("리셋")]
        public async Task RestCommand()
        {
            waitList.Clear();


            await Context.Channel.SendMessageAsync("대기 명단 리셋됨");
        }

        [Command("명령어")]
        public async Task CommandCommand()
        {
           
            await Context.User.SendMessageAsync("!체크 /닉네임/, !취소 /닉네임/ or /번호/, !양보 /닉네임/, !대기, !리셋");
            //await Context.Channel.SendMessageAsync("!체크 /닉네임/, !취소 /닉네임/ or /번호/, !양보 /닉네임/, !대기, !리셋");
        }
    }
}
