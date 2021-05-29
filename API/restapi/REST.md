# A REST-ful walkthrough

This post describes a sample application that captures timesheet data from people and implements a resource-based routing workflow. This isn't a real application, but it does form the basis for one. Any references to existing products is purely accidental. You can think of the model presented here as "pure REST" if you'd like. I try to avoid the term "pure". Check out the definition of HATEOAS on wikipedia for a better idea of where this post is going and look the the Richardson Maturity Model over at Martin Fowler's site.

...oh yeah, and no picking on the code here, it's for clarity not style...

There's an underlying assumption about REST, in fact it's core to the entire REST architecture: the service owns all URLs and the client shouldn't make any assumptions, in general, about them.

So, in order to construct a client that consumes the timesheet service, we only need to agree on a few things. The first is the starting point for the service and what resource that starting point will provide to the client. So, let's just jump in and ask the service what we can do. Think of this as the service-equivalent of a web site's index.html.

In the client we construct an AJAX request

```javascript
$.ajax({
  url: '/',
  data: null,
  type: 'GET',
  dataType: 'json',
  beforeSend: function (req) {
    req.setRequestHeader('Accept', 'application/json');
  },
  success: function (root) {
    // process root node
  },
});
```

Which results in a GET to the service.

```http
GET / HTTP/1.1
X-Requested-With: XMLHttpRequest
Accept: application/com.my-company.my-product.root+json
```

The response includes the correct `Content-Type` matching what we asked for and what we expect. This indicates the type of thing we passed back to client. Versioning can take place in either the `Content-Type` name or in a property of the returned object.

```bash
curl http://localhost:5000/ \
  --header "X-Request-Id: `uuidgen`" \
  --dump-header -
```

> Ok, before we go too much further let me explain what's happening in the line above. `curl` is a tool for making HTTP requests via the command line. In this case we're making a simple HTTP request to `http://localhost:5000/` and dumping all the HTTP response headers (right below, but use `--trace-ascii -` if you want to see _everything_ to and from the server). We'll see some more curl options as we're going.

```http
HTTP/1.1 200 OK
Date: Mon, 12 Apr 2021 21:45:01 GMT
Content-Type: application/com.my-company.my-product.root+json; charset=utf-8
Server: Kestrel
Content-Length: 718

{
  "timesheets": {
    "method": "GET",
    "type": "application/com.my-company.my-product.timesheets",
    "rel": "timesheets",
    "href": "/timesheets"
  },
  "version": "0.1"
}
```

Now, look at the navigation link for the area in which you're interested and ask the service for that. In this case we're interested in the timesheets-related resources.

```javascript
if (root.timesheets.rel == 'timesheets') {
  // load the data for this page
  $.ajax({
    url: root.timesheets.href,
    data: null,
    type: 'GET',
    dataType: 'json',
    beforeSend: function (req) {
      req.setRequestHeader('Accept', 'application/com.my-company.my-product.timesheets+json');
    },
    success: function (list) {
      $.each(list, function (key, item) {
        addItem(item);
      });
    },
  });
} else {
  $('body').append(
    '<div id="no-timesheets-link" class="ui-widget">' +
      '  <p>' +
      '    <strong>Error</strong> No timesheets link present at this site' +
      '  </p>' +
      '</div>'
  );
}
```

We get the root node from the service, notice the `Content-Type` is what we requested in the initial call.

```bash
curl http://localhost:5000/timesheets --header "X-Request-Id: `uuidgen`" --dump-header -
```

```http
GET /timesheets HTTP/1.1
X-Requested-With: XMLHttpRequest
Accept: application/com.my-company.my-product.timesheets+json
```

```http
HTTP/1.1 200 OK
Content-Type: application/com.my-company.my-product.timesheets+json; charset=utf-8
Content-Length: 2

[]
```

In this case we have the complete set of timesheets that the service knows about turns out there are none returned, so we see an empty array [].

Now, let's add a timesheet, by convention we accept a POST to the /timesheets URI to ask the service to perform whatever business logic is necessary for opening a new timesheet.

Notice in this case that the `Content-Type` is a generic JSON request. This is not necessary, and instead we could have supplied the type name for the "add timesheet" object template.

```bash
curl http://localhost:5000/timesheets \
    -X POST \
    --header "Content-Type: application/json" \
    --header "X-Request-Id: `uuidgen`" \
    --data '{ "id": 1 }' \
    --dump-header -
```

```http
POST /timesheets HTTP/1.1
Accept: application/com.my-company.my-product.timesheet+json
Content-Type: application/json
X-Requested-With: XMLHttpRequest
Content-Length: 15

{ "id": 1 }
```

By REST rules the service responds with a timesheet object as indicated in the `Content-Type` header.

```http
HTTP/1.1 200 OK
Content-Type: application/com.my-company.my-product.timesheet+json; charset=utf-8
Content-Length: 1104

{
  "opened": "2020-01-09 19:53:08Z",
  "employee": 0,
  "id": "6a0189d7-d8a8-45e9-b3be-63657459fca4",
  "status": "draft",
  "actions": [
    {
      "method": "post",
      "type": "application/com.my-company.my-product.timesheet-cancellation+json",
      "rel": "cancel",
      "href": "/timesheets/6a0189d7-d8a8-45e9-b3be-63657459fca4/cancellation"
    },
    {
      "method": "post",
      "type": "application/com.my-company.my-product.timesheet-submittal+json",
      "rel": "submit",
      "href": "/timesheets/6a0189d7-d8a8-45e9-b3be-63657459fca4/submittal"
    },
    {
      "method": "post",
      "type": "application/com.my-company.my-product.timesheet-line+json",
      "rel": "recordLine",
      "href": "/timesheets/6a0189d7-d8a8-45e9-b3be-63657459fca4/lines"
    }
  ],
  "documentation": [
    {
      "method": "get",
      "type": "application/com.my-company.my-product.timesheet-transitions+json",
      "rel": "transitions",
      "href": "/timesheets/6a0189d7-d8a8-45e9-b3be-63657459fca4/transitions"
    }
  ],
  "version": "timecard-0.1"
}
```

In addition to the timesheet data there are a number of additional elements that are of interest. In this case there are two arrays, `documentation` and `actions`. The `documentation` array contains link elements that tell the client where to look for additional information about this timesheet.

The first thing we want to do is to read any timesheet lines, so we construct a request to the service. We do this by looking at the `documentation` links to find the one with the `lines` relationship to this timesheet.

```javascript
var itemsLink;
$.each(i.documentation, function (key, doc) {
  if (doc.rel == 'lines') itemsLink = doc.href;
});
```

Then we make the request to the service for those lines, notice again we set the `Content-Type` to indicate that we're looking for a specific format and version for the lines. Only then can we make the request to load the line items.

```javascript
$.ajax({
  url: itemsLink,
  data: null,
  type: 'GET',
  dataType: 'json',
  beforeSend: function (req) {
    req.setRequestHeader('Accept', 'application/com.my-company.my-product.timesheet-lines+json');
  },
  success: function (lines) {
    $('#' + i.id.value)
      .next('div')
      .html(bindHandler(addTimesheetLineItemsToPage(i, lines)));
  },
});
```

```bash
curl --header "X-Request-Id: `uuidgen`" http://localhost:5000/timesheets/6a0189d7-d8a8-45e9-b3be-63657459fca4/lines
```

The service responds with exactly what we asked for, a collection of line items for this timesheet.

```http
HTTP/1.1 200 OK
Content-Type: application/com.my-company.my-product.timesheet-lines+json
Content-Length: 2

[]
```

In this case, since this is a new timesheet there are no line items, so let's add one. By convention we POST to the lines URL that we found in the timesheet `documentation` link.

So, let's do just that. We'll create a new request and POST it to the lines URL.

```javascript
$.ajax({
  url: '/timesheets/dat-4035447427/lines',
  data: {
    week: $('#week').val(),
    year: $('#year').val(),
    day: $('#day').val(),
    hours: parseFloat($('#hours').val()),
    project: $('#project').val(),
  },
  type: 'POST',
  dataType: 'json',
  beforeSend: function (req) {
    req.setRequestHeader('Accept', 'application/com.my-company.my-product.timesheet-line+json');
  },
  success: function (item) {
    appendLine(lastItem, item);
  },
});
```

Which results in the following HTTP request.

```bash
curl http://localhost:5000/timesheets/dat-1522772509/lines \
    -X POST \
    --header "Content-Type: application/json" \
    --header "X-Request-Id: `uuidgen`" \
    --data '{ "week": 2, "year": 2018, "day": "wednesday", "hours": 8, "project": "SEGR 5240" }' \
    --dump-header -

curl http://localhost:5000/timesheets/dat-1522772509/lines \
    -X POST \
    --header "Content-Type: application/json" \
    --header "X-Request-Id: `uuidgen`" \
    --data '{ "week": 2, "year": 2018, "day": "thursday", "hours": 8, "project": "SEGR 5240" }' \
    --dump-header -

curl http://localhost:5000/timesheets/dat-1522772509/lines \
    -X POST \
    --header "Content-Type: application/json" \
    --header "X-Request-Id: `uuidgen`" \
    --data '{ "week": 2, "year": 2018, "day": "friday", "hours": 8, "project": "SEGR 5240" }' \
    --dump-header -
```

```http
POST /timesheets/dat-4035447427/lines HTTP/1.1
Accept: application/com.my-company.my-product.timesheet-line+json
Content-Type: application/json
X-Requested-With: XMLHttpRequest
Content-Length: 78

{
  "week": 2,
  "year": 2012,
  "day": "thursday",
  "hours": 8,
  "project": "project"
}
```

In this case we've asked for the annotated timesheet line as indicated by the `Accept` header that we've added. The service responds with our newly-added timesheet line.

```http
HTTP/1.1 200 OK
Date: Tue, 03 Apr 2018 16:26:12 GMT
Content-Type: application/com.my-company.my-product.timesheet-line+json; charset=utf-8
Transfer-Encoding: chunked

{
  "recorded": "2018-04-03 16:26:13Z",
  "workDate": "2018-01-01 00:00:00Z",
  "lineNumber": 0.0,
  "uniqueIdentifier": "38ecd809-9f08-4c6e-87d6-ac03f0881405",
  "periodFrom": "0001-01-01 00:00:00Z",
  "periodTo": "0001-01-01 00:00:00Z",
  "version": "line-0.1",
  "week": 1,
  "year": 2018,
  "day": "monday",
  "hours": 8.0,
  "project": "SEGR 5240"
}
```

Now, let's say we want to submit this timesheet for approval. To do that we look at the `action` link collection to find the `submit` relationship and it's corresponding href value. If we look at the collection

```json
"actions": [
  {
    "method": "POST",
    "type": "application/com.my-company.my-product.timesheet-submittal",
    "rel": "submit",
    "href": "/timesheets/dat-4035447427/submittal"
  },
  {
    "method": "POST",
    "type": "application/com.my-company.my-product.timesheet-cancellation",
    "rel": "cancel",
    "href": "/timesheets/dat-4035447427/cancellation"
  }
]
```

Theres an action for cancelling the request and an action for submitting it. Each action tells us the name of the resource to create (in the href) and the type of resource to send (in the type).

We then construct an AJAX request to the server.

```javascript
$.ajax({
  url: '/timesheets/dat-4035447427/submittal',
  data: data,
  type: 'POST',
  dataType: 'json',
  beforeSend: function (req) {
    req.setRequestHeader('Accept', 'application/com.my-company.my-product.timesheet+json');
  },
  success: function (item, status, xhr) {
    if (verb == 'closure') removeItem(item);
    else replaceItem(item, false);
  },
  error: function (xhr, status, error) {
    var o = JSON.parse(xhr.responseText);
    alert(o.message);
  },
});
```

Which looks like

```http
POST /timesheets/dat-4035447427/submittal HTTP/1.1
Accept: application/com.my-company.my-product.timesheet+json
Content-Type: application/json
X-Requested-With: XMLHttpRequest
Content-Length: 14

{ "id": 1 }
```

Notice that we've asked the service to respond back with a timesheet object by providing a specific `Accept` header. the service responds with the timesheet.

```http
HTTP/1.1 200 OK
Content-Type: application/com.my-company.my-product.timesheet+json; charset=utf-8
Content-Length: 1487

{
  "id": 1,
  "id": {
    "area": "dat",
    "number": "4035447427",
    "value": "dat-4035447427"
  },
  "status": "submitted",
  "opened": "2012-01-12T17:00:03Z",
  "uniqueIdentifier": "cae499ca-3c50-4249-9685-88d24c263534",
  "actions": [
    {
      "method": "POST",
      "type": "application/com.my-company.my-product.timesheet-rejection",
      "rel": "reject",
      "href": "/timesheets/dat-4035447427/rejection"
    },
    {
      "method": "POST",
      "type": "application/com.my-company.my-product.timesheet-approval",
      "rel": "approve",
      "href": "/timesheets/dat-4035447427/approval"
    },
    {
      "method": "POST",
      "type": "application/com.my-company.my-product.timesheet-cancellation",
      "rel": "cancel",
      "href": "/timesheets/dat-4035447427/cancellation"
    }
  ],
  "documentation": [
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-submittal",
      "rel": "submittal",
      "href": "/timesheets/dat-4035447427/submittal"
    },
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-transitions",
      "rel": "transitions",
      "href": "/timesheets/dat-4035447427/transitions"
    },
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-lines",
      "rel": "lines",
      "href": "/timesheets/dat-4035447427/lines"
    }
  ],
  "version": "0.1"
}
```

This is the same timesheet as indicated by the `id` property, but it now includes different `action` and `documentation` link collections. The business process that governs how timesheets behave in our system disallows submitting an already-submitted timesheet, so there's no `submit` relationship, it's been replaced by `approve` and `reject`.

Notice also that the `documentation` link collection includes a new resource for `submittal`. This is an indicator to consumers of this timesheet that it's been submitted, and any interested party can ask for that resource (as documentation / proof).

Let's break the rules and see what happens if we try to resubmit the timesheet we'll POST another submit.

```http
POST /timesheets/dat-4035447427/submittal HTTP/1.1
Accept: application/com.my-company.my-product.timesheet+json
Content-Type: application/json
X-Requested-With: XMLHttpRequest
Content-Length: 14

{ "id": 1 }
```

The server responds with an HTTP 400 status along with some additional information about what went wrong. We can decide how to handle this case by looking at the `Content-Type`. In this case the server has handed back a type that we recognize, so we go ahead and handle the error locally. Note that this is not necessarily the correct status code. a `409 Conflict` or `412 Precondition` Failed might make more sense for your application.

```http
HTTP/1.1 400 Bad Request
Content-Type: application/com.my-company.my-product.simple-error+json
Content-Length: 45

{
  "message": "Invalid state transition"
}
```

Next, we will cancel this timesheet (in this case the same person is doing the cancelling, ignore that for now). The cancellation `Content-Type` requires a reason, so provide that as well.

```http
POST /timesheets/dat-4035447427/cancellation HTTP/1.1
Accept: application/com.my-company.my-product.timesheet+json
Content-Type: application/json
X-Requested-With: XMLHttpRequest
Content-Length: 102

{
  "id": 1,
  "reason": "Not enough information provided. Please complete the timesheet and resubmit."
}
```

And, once again, the service responds with the updated timesheet.

```http
HTTP/1.1 200 OK
Content-Type: application/com.my-company.my-product.timesheet+json; charset=utf-8
Content-Length: 1107

{
  "id": 1,
  "id": {
    "area": "dat",
    "number": "4035447427",
    "value": "dat-4035447427"
  },
  "status": "canceled",
  "opened": "2012-01-12T17:00:03Z",
  "uniqueIdentifier": "cae499ca-3c50-4249-9685-88d24c263534",
  "actions": [
    {
      "method": "POST",
      "type": "application/com.my-company.my-product.timesheet-closure",
      "rel": "close",
      "href": "/timesheets/dat-4035447427/closure"
    }
  ],
  "documentation": [
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-cancellation",
      "rel": "cancellation",
      "href": "/timesheets/dat-4035447427/cancellation"
    },
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-transitions",
      "rel": "transitions",
      "href": "/timesheets/dat-4035447427/transitions"
    },
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-lines",
      "rel": "lines",
      "href": "/timesheets/dat-4035447427/lines"
    }
  ],
  "version": "0.1"
}
```

And finally, the only action we can perform against our timesheet is indicated in the `actions` link collection, and that says we can close this timesheet. let's do that and see what happens.

```http
POST /timesheets/dat-4035447427/closure HTTP/1.1
Accept: application/com.my-company.my-product.timesheet+json
Content-Type: application/json
X-Requested-With: XMLHttpRequest
Content-Length: 14

{ "id": 1 }
```

And we get back the timesheet document from the service.

```http
HTTP/1.1 200 OK
Content-Type: application/com.my-company.my-product.timesheet+json; charset=utf-8
Content-Length: 885

{
  "id": 1,
  "id": {
    "area": "dat",
    "number": "4035447427",
    "value": "dat-4035447427"
  },
  "status": "closed",
  "opened": "2012-01-12T17:00:03Z",
  "uniqueIdentifier": "cae499ca-3c50-4249-9685-88d24c263534",
  "documentation": [
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-closure",
      "rel": "closure",
      "href": "/timesheets/dat-4035447427/closure"
    },
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-transitions",
      "rel": "transitions",
      "href": "/timesheets/dat-4035447427/transitions"
    },
    {
      "method": "GET",
      "type": "application/com.my-company.my-product.timesheet-lines",
      "rel": "lines",
      "href": "/timesheets/dat-4035447427/lines"
    }
  ],
  "version": "0.1"
}
```

But notice in this case that the `actions` link collection is not present. That's because the underlying business process says that no additional work can happen to a closed timesheet. In fact, if we were to go back and request the timesheet list from `/timesheets`, we'd get back an empty array since there are no non-closed timesheets available.
