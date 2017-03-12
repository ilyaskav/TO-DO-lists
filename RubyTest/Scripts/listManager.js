$(function () {
    var onClick_ButtonAddList = function (e) {
        $(e.target).button('loading');

        $.ajax({
            method: 'POST',
            url: '/api/list',
            statusCode: {
                401: function () {
                    window.location.href = '/Account/Login';
                }
            }
        })
            .success(function (data, response) {
                toastr.success('New list has been added successfully');

                loadLists();
                $(e.target).button('reset');
            }

            ).error(function (response) {
                toastr.error(response.responseText);

                $(e.target).button('reset');
            });
    }

    var onClick_ButtonAddTask = function (e) {
        $(e.target).button('loading');

        // примитивная валидация
        var name = $(e.target).parent().prev().children('input[type="text"]').val();
        var projectId = $(e.target).parent().prev().children('input[type="hidden"]').val();

        if (name == '' || name.length <= 2) {
            toastr.error('Enter corrent task with 2 or more characters');
            $(e.target).button('reset');
            return;
        }
        if (projectId == '' || isNaN(projectId)) {
            toastr.error('Something wrong with your project. Try to refresh page');
            $(e.target).button('reset');
            return;
        }

        $.ajax({
            method: 'POST',
            url: '/api/task',
            data: {
                Name: name,
                Status: 'Created',
                ProjectId: projectId
            },
            statusCode: {
                401: function () {
                    window.location.href = '/Account/Login';
                }
            }
        })
            .success(function () {
                loadTasks(e.target);

                $(e.target).parent().prev().children('input[type="text"]').val('');
                toastr.success('New task has been added successfully');
                $(e.target).button('reset');
            }

            ).error(function (response) {
                toastr.error(response.responseText);

                $(e.target).button('reset');
            });

    }

    var onClick_DeleteTask = function (e) {
        var sender = $(e.target);
        var taskId = sender.closest('.task').find('input[type="checkbox"]').val();
        var projectId = sender.closest('.task-list').prev().find('input[type="hidden"]').val();
        var status = sender.closest('.controls').children('input[type="hidden"]').val();

        if (taskId=='' || taskId==undefined) {
            toastr.error("Cannot identify task id");
            return;
        }
        if (projectId == '' || projectId == undefined) {
            toastr.error("Cannot identify project id");
            return;
        }
        if (status == '' || status == undefined) {
            toastr.error("Cannot identify task status");
            return;
        }

        $.ajax({
            method: 'DELETE',
            url: '/api/task',
            data: {
                TaskId: taskId,
                Status: status,
                ProjectId: projectId
            },
            statusCode: {
                401: function () {
                    window.location.href = '/Account/Login';
                }
            }
        }).success(function () {
            loadTasks(e.target);
            toastr.success("Task has been deleted");
        }).error(function (response) {
            toastr.error(response.responseText);
        });
    }

    var onClick_DeleteList = function (e) {
        var sender = $(e.target);
        var projectId = sender.closest('.title').next().find('input[type="hidden"]').val();

        if (projectId == '' || projectId == undefined) {
            toastr.error("Cannot identify project id");
            return;
        }

        var confirmDelete = confirm("Are you sure you want to delete this list with all tasks that it has?");
        if (!confirmDelete) return;

        $.ajax({
            method: 'DELETE',
            url: '/api/list/'+projectId,
        }).success(function () {
            loadLists();
            toastr.success("List was successfully deleted");
        }).error(function (response) {
            toastr.error(response.responseText);
        });
    }

    var loadTasks = function (sender) {
        if ($(sender).is('button'))
            var projectId = $(sender).parent().prev().children('input[type="hidden"]').val();
        else var projectId = $(sender).closest('.task-list').prev().find('input[type="hidden"]').val();
        if (projectId == '' || isNaN(projectId)) {
            toastr.error('Something wrong with your project. Try to refresh page');
            $(sender).button('reset');
            return;
        }

        $.get('/api/task/' + projectId)
            .success(function (data) {
                if ($(sender).is('button'))
                    var container = $(sender).closest('.add-task').next();
                else var container = $(sender).closest('.task-list');
            container.empty();
            var html = addTasksToHtml(data);
            container.append('<hr/>' + html);

            //toastr.success('Tasks from this project has been reloaded');
        })
        .error(function(response){
            toastr.error(response.responseText);
        });
    }

    var loadLists = function (e) {
        $.get("/api/list")
            .success(function (data) {
                var container = $('#list-container');

                if (data.length == 0) {
                    container.empty();

                    toastr.info('No projects yet! Add one.');
                    return;
                }

                container.empty();
                for (var i = 0; i < data.length; i++) {
                    var list = '';
                    list += ('<div class="container task-wrap">');
                    list += ('<div class="row title">' +
                             '<div class="col-md-1"></div> <div class="col-md-9">' +
                             '<label >' + data[i].Name + '</label>' +
                             '</div> <div class="col-md-2 controls">  <div class="delete-list"></div><hr class="separator m-0 s2" /><div class="edit-list" data-toggle="modal" data-target="#myModal"></div></div> </div> ');

                    list += ('<div class="row add-task"> <div class="col-md-1">  </div>' +
                             '<div class="col-md-9 p-r-0 p-l-5"> <input type="text" name="taskName" placeholder="Start typing here to create a task..." class="form-control" /> <input type="hidden" name="listId" value='+ data[i].Id + ' />'+
                             '</div> <div class="col-md-2 p-r-10 p-l-0"> <button class="form-control">Add Task</button> </div> </div>');

                    list += ('<div class="task-list"> <hr />');

                    list += addTasksToHtml(data[i].Tasks);
                    container.append(list);
                }
            })
        .error(function (response) {
            toastr.error(response.responseText);

            $(e.target).button('reset');
        });
    }
    var addTasksToHtml = function (data) {
        var list = '';
        var checked = '';

        if (data.length <= 0) {
            list += '<div class="row task"> <div class="col-md-1"></div>' +
                    '<div class="col-md-9 border-line">' +
                    '<p class="text-left">No tasks yet. Add one in the field above and click button Add Task</p></div>' +
                    '<div class="col-md-2 controls"> </div> </div>';
        }
        else
            for (var i = 0; i < data.length; i++) {
                if (data[i].Status === "Done") checked = 'checked';
                else checked = '';
                list += '<div class="row task"> <div class="col-md-1 status">' +
                      '<label><input type="checkbox" class="checkbox" value="' + data[i].TaskId + '" ' + checked + ' /><i></i></label> </div>' +
                      '<div class="col-md-9 border-line task-text"> <p class="text-left">' + data[i].Name + '</p>' +
                      '</div> <div class="col-md-2 controls"><input type="hidden" name="taskStatus" value="'+data[i].Status +'" /> <div>' +
                      '<div class="move-task"> </div> <hr class="separator m-0" />' +
                      '<div class="edit-task" data-toggle="modal" data-target="#myModal"> </div> <hr class="separator m-0" />' +
                      '<div class="delete-task"></div> </div> </div></div>';
            }

            list += ('</div> </div> </div>');
            return list;
    }
    
    loadLists();
    $('#btn-add-project').on('click', onClick_ButtonAddList);
    $(document).on('click', '.delete-task', onClick_DeleteTask);
    $(document).on('click', '.delete-list', onClick_DeleteList);
    $(document).on('click', '.add-task button', onClick_ButtonAddTask);
    $(document).on({
        mouseenter: function (e) {
            $(this).find('.controls').css('display', 'block');
        },
        mouseleave: function (e) {
            $(this).find('.controls').css('display', 'none');
        }
    }, '.task, .title');
    $(document).on('click', '.task > .task-text, .task > .status', function (e) {
        var checkbox = $(e.target).closest('.task').find('input[type="checkbox"]');
        checkbox.prop("checked", !checkbox.prop("checked"));
        var isChecked = checkbox.is(':checked'); //checkbox.prop("checked", !checkbox.prop("checked"));
        var taskId = checkbox.val();
        var projectId = $(this).closest('.task-list').prev().find('input[type="hidden"]').val();

        if (projectId == '' || projectId == undefined) {
            toastr.error("Cannot identify project id");
            $(this).button('reset');
            return;
        }

        if (taskId == '' || taskId == undefined) {
            toastr.error("Cannot identify task id");
            $(this).button('reset');
            return;
        }

        //if (isChecked) {
        //    var taskStatus = $(e.target).closest('.task').find('input["hidden"]');
        //}
        $.ajax({
            method: 'PUT',
            url: '/api/task/status',
            data: {
                ProjectId: projectId,
                TaskId: taskId,
                Status: isChecked===false? "Created":"Done"
            }
        }).success(function () {
            loadTasks(e.target);
            toastr.success("Task status was changed");
        }).error(function (response) {
            toastr("Error with task status update");
        });

    });
    $('#myModal').on('show.bs.modal', function (event) {
        var modal = $(this)
        var sender = $(event.relatedTarget) // Button that triggered the modal
        var senderClass = sender.attr('class') // Extract info from data-* attributes
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.

        var textarea = modal.find('#changed-text');
        var saveChangesButton= modal.find('.btn-primary');
        textarea.val('');

        if (senderClass === 'edit-task') {
            var currentText = sender.closest('.task').find('p').text();
            if (currentText != undefined && currentText != '') textarea.val(currentText);

            saveChangesButton.on('click', function () {
                $(this).button('loading');
                var currentText = sender.closest('.task').find('p').text();
                var taskId = sender.closest('.task').find('input[type="checkbox"]').val();
                var projectId = sender.closest('.task-list').prev().find('input[type="hidden"]').val();
                var status = sender.closest('.controls').children('input[type="hidden"]').val();

                if (textarea.text == '' || textarea.val().length < 2) {
                    toastr.error("Enter 2 or mode digits");
                    $(this).button('reset');
                    return;
                }
                if (taskId == '' || taskId == undefined) {
                    toastr.error("Cannot identify task id");
                    $(this).button('reset');
                    return;
                }
                if (projectId == '' || projectId == undefined) {
                    toastr.error("Cannot identify project id");
                    $(this).button('reset');
                    return;
                }
                if (status == '' || status == undefined) {
                    toastr.error("Cannot identify task status");
                    $(this).button('reset');
                    return;
                }
                if (textarea.val() === currentText) {
                    $(this).button('reset');
                    modal.modal('hide');
                    return;
                }

                $.ajax({
                    method: 'PUT',
                    url: '/api/task/',
                    data: {
                        TaskId: taskId,
                        Name: textarea.val(),
                        ProjectId: projectId,
                        Status: status
                    },
                    statusCode: {
                        401: function () {
                            window.location.href = '/Account/Login';
                        }
                    }
                }).success(function () {
                    loadTasks(event.relatedTarget);
                    toastr.success('Task successfully updated');
                    saveChangesButton.button('reset');
                    modal.modal('hide');

                }).error(function (response) {
                    saveChangesButton.button('reset');
                    toastr(response.responseText);
                });
            });
            
        }
        else if (senderClass === 'edit-list') {
            var currentText = sender.closest('.title').find('label').text();
            if (currentText != undefined && currentText != '') textarea.val(currentText);

            saveChangesButton.on('click', function () {
                $(this).button('loading');
                var currentText = sender.closest('.title').find('label').text();
                var projectId = sender.closest('.title').next().find('input[type="hidden"]').val();

                if (projectId == '' || projectId == undefined) {
                    toastr.error("Cannot identify project id");
                    $(this).button('reset');
                    return;
                }
                if (textarea.text == '' || textarea.val().length < 2) {
                    toastr.error("Enter 2 or mode digits");
                    $(this).button('reset');
                    return;
                }
                if (textarea.val() === currentText) {                   
                    $(this).button('reset');
                    modal.modal('hide');
                    return;
                }

                $.ajax({
                    method: 'PUT',
                    url: '/api/list',
                    data: {
                        ProjectId: projectId,
                        Name: textarea.val()
                    },
                    statusCode: {
                    401: function () {
                        window.location.href = '/Account/Login';
                    }
                }
                }).success(function () {
                    loadLists(event.relatedTarget);
                    saveChangesButton.button('reset');
                    modal.modal('hide');
                    toastr.success("Task has been changed succesfully");

                }).error(function (response) {
                    saveChangesButton.button('reset');
                    toastr.error(response.responseText);
                });
            });
        }
    });
    $('#myModal').on('hide.bs.modal', function () {
        $(this).find('.btn-primary').off('click');
    })
});