System.register(["cytoscape"], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    function sayHello() {
        var compiler = document.getElementById("compiler").value;
        var framework = document.getElementById("framework").value;
        return "Hello from " + compiler + " and " + framework + "!";
    }
    function main(data, triggers, options) {
        var cy = cytoscape_1["default"]({
            container: document.getElementById('cy'),
            style: [
                {
                    selector: 'node',
                    style: {
                        'background-color': '#666',
                        'label': 'data(id)'
                    }
                },
                {
                    selector: 'edge',
                    style: {
                        'width': 3,
                        'line-color': '#ccc',
                        'mid-target-arrow-color': '#ccc',
                        'mid-target-arrow-shape': 'triangle',
                        'curve-style': 'bezier'
                    }
                }
            ]
        });
        var roots = [];
        var qtipname = function (text) { return { content: text, position: { my: 'top center', at: 'bottom center' }, style: { classes: 'qtip-bootstrap', tip: { width: 16, height: 8 } }, show: { event: 'mouseover' }, hide: { event: 'mouseout' } }; };
        data.forEach(function (element) {
            var name = element.ID;
            var desc;
            if (element.Desc === '') {
                desc = element.ID;
            }
            else {
                desc = element.Desc;
            }
            var node = cy.add({ group: 'nodes', data: { id: name } });
            node.qtip(qtipname(desc));
        });
        triggers.forEach(function (event) {
            var parentID = event[0];
            event[1].forEach(function (immediates) {
                immediates.forEach(function (target) {
                    var childID = target;
                    cy.add({ group: 'edges', data: { source: parentID, target: childID } });
                });
            });
        });
        options.forEach(function (event) {
            var parentID = event[0];
            event[1].forEach(function (option) {
                var optionName = option[0];
                option[1].forEach(function (target) {
                    if (cy.getElementById(target).length > 0) {
                        var edge = cy.add({ group: 'edges', data: { source: parentID, target: target } });
                        if (optionName !== "") {
                            edge[0].qtip(qtipname(optionName));
                        }
                    }
                });
            });
        });
        cy.fit();
        var layout = cy.layout({ name: 'dagre' });
        layout.run();
        cy.fit();
    }
    function go(file) {
        $.ajax({
            url: "GetData",
            data: { "file": file }
        })
            .done(function (data) {
            main(JSON.parse(data.item1), JSON.parse(data.item2), JSON.parse(data.item3));
        });
    }
    exports_1("go", go);
    var cytoscape_1;
    return {
        setters: [
            function (cytoscape_1_1) {
                cytoscape_1 = cytoscape_1_1;
            }
        ],
        execute: function () {
        }
    };
});