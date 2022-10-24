using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenData.WebUI.Models;

namespace OpenData.WebUI.Controllers
{
    public class AjaxController : Controller
    {

        /// <summary>
        /// Загрузка тем для сообщения
        /// будет тоже происходить через
        /// ajax-запрос с формы
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadSubjects()
        {
            List<string> subjects = new List<string>() {
		        "Заявка на регистрацию блога на calabonga.net",
		        "Связь с администратором",
		        "Связь с блогером",
		        "Вопрос об копирайтах",
		        "Благодарственное письмо",
		        "Желание поблагодарить материально"
	        };
            return Json(subjects.ToArray(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Отправленная с формы модель
        /// будет приходить сюда
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendFeedback(FeedbackViewModel model)
        {
            return Json("");
        }
    }
}
