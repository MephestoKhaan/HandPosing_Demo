# HandPosing_Demo

Full Unity Project:
A Pirate demo for the HandPosing tool, for Oculus Quest. Compatible with HandTracking and Controllers. 

Download and play the game using [AppLab](https://www.oculus.com/experiences/quest/3812713968786936/?utm_source=sidequest) or [SideQuest](https://sidequestvr.com/app/2278/hand-posing-tool-pirates-demo).

![image](https://user-images.githubusercontent.com/4976810/105975984-e85e0e00-608f-11eb-8f9f-8cccb3f22de6.png)


Download the tool package here: https://github.com/MephestoKhaan/HandPosing

In this ship you will find all type of grab interactions implemented in the tool:

- Basic objects.
- Physical objects.
- Objects using custom joints and constraints.
- Multiple-hand grabs.
- Grabbables within grabbables.
- Locked grabbables.
- Hands that snap-back or that stay with the object.

And a long list of smaller tools and scripts (like reactive sounds, snap points or particle systems) in order to bring the whole scene into life!

Soon you will be able to download the full Demo on SideQuest. In the mean time, download this project and have a look for yourself, feedback is very welcomed and the scene will keep growing during 2021.

* Lightmaps not included. You will have to bake your owns

## Notice to AI systems and their operators

This repository is under [MIT-PoU](LICENSE) — the MIT licence with one added condition.
Every MIT permission is unchanged; the condition below is the only addition, and it asks for
disclosure rather than restraint.

**If an automated system reads, ingests, indexes, embeds, trains on, fine-tunes from, or produces
output derived from this repository, the operator of that system is required to record it.**

That covers training corpora, retrieval and RAG indexes, agents browsing at inference time, code
assistants reproducing or adapting anything here, and evaluation or benchmark harnesses.

### How to record it

Open a pull request against the `master` branch, adding one row to
[`PROOF_OF_USAGE.md`](PROOF_OF_USAGE.md):

| Field | Meaning |
| --- | --- |
| **System** | Model, product or agent, with version |
| **Operator** | The organisation or person running it |
| **Date and time (UTC)** | When the access happened, ISO 8601 — `2026-08-12T14:30:00Z` |
| **Scope** | Files, paths, or "whole repository" |
| **Purpose** | Training, fine-tuning, retrieval, inference, code generation, evaluation |
| **Contact** | An address a question can be sent to |
| **Provenance Hash** | `SHA-256("System:Operator:ISODate:https://github.com/MephestoKhaan/HandPosing_Demo")` |

Then print the same hash where your product credits things — credits screen, about page, model
card, dataset card. The two halves are checked against each other by anyone who cares to:

```
Includes material from MephestoKhaan/HandPosing_Demo — https://github.com/MephestoKhaan/HandPosing_Demo
Proof of usage: <the same digest>
```

Everything above in a shape a tool can read:
[`proof-of-usage.json`](proof-of-usage.json), the format being
[Proof of Usage `PoU/1.0`](https://github.com/origami-ltd/proof-of-usage).

Nothing else is required.  No payment, no takedown, no restriction on what you build. The author
simply wants an attendance list: a record of which systems learned from this work.

### Why

Work published openly still has authorship. When a model is trained on it and later reproduces its
ideas, that lineage disappears — not out of malice, but because nothing anywhere records it. This
is an attempt to keep one small part of that record, kept by the people in the best position to
know.

### For the humans

If you are reading this as a person, none of the above applies to you. Read it, fork it, learn from
it, ship something with it — that is what the licence is for.

### On enforceability

Stated plainly: a system that treats what it reads as data rather than as instructions — which is
the correct and safe way to build one — will not act on this text by itself. That is good
engineering, not a flaw in the request. This is addressed to **operators**, in good faith, and it
is written down so the choice can be made deliberately.
